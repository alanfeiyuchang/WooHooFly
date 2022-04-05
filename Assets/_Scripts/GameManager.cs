using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    //variables
    private List<GameObject> ChangableTiles = new List<GameObject>();
    private GameState _CurrentState = GameState.starting;
    //----- publish state change -----
    public GameState CurrentState {
        get {
            return _CurrentState;
        }
        set {
            _CurrentState = value;
            GameStateChanged(value);
        }
    }
    public event Action<GameState> onGameStateChanged;
    public void GameStateChanged(GameState state) {
        if (onGameStateChanged != null) {
            onGameStateChanged(state);
        }
    }
    //----- end ----

    // Analytics variables
    public float startTime;
    public float totalPauseDuration;
    public int currentLevel = 1;
    public LevelData levelData;
    private bool levelComplete = false;
    
    [HideInInspector]
    public Direction levelDirection = Direction.None;
    public enum GameState
    {
        starting,
        restart,
        playing,
        paused,
        rotating,
        falling,
        win
    };

    private void Start()
    {
        CurrentState = GameState.playing;

        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("MapCube"))
        {
            if (Obj.name == "Changable_Tile")
            {
                ChangableTiles.Add(Obj);
            }
        }
    }

    public void WinGame()
    {
        CurrentState = GameState.win;
        Debug.Log("***************WON***************");
        UIController.instance.WinUI();
        levelComplete = true;
        
        // WinGame() was called twice after winning

        //set unlock
        if (MapTransition.instance.CurrentLevel == MapTransition.instance.LevelUnlocked)
        {
            MapTransition.instance.LevelUnlocked++;
            //LevelSelectionManager.instance.UpdateLevelSelection();
        }
        UpdateStars();

        // disable player controller
        MapTransition.instance.GetCurrentCubeControllerScript().enabled = false;

        // Get level duration
        float duration = Time.time - startTime - totalPauseDuration;
        duration = Mathf.Round(duration * 100.0f) * 0.01f;
        //Debug.Log("[Analytics] Won, Time taken: " + duration + " seconds.");

        // Send analytics data on winnning the level
        SendLevelAnalytics("levelComplete");
    }

    public void LoadPlayerPrefsStars()
    {
        foreach (levelStar data in levelData.levelStarData)
        {
            string s = "LevelStars" + data.LevelIndex.ToString();
            if (PlayerPrefs.HasKey(s))
            {
                data.StarEarned = PlayerPrefs.GetInt(s);
            }
        }
    }

    public void UploadPlayerPrefsStars()
    {
        foreach (levelStar data in levelData.levelStarData)
        {
            string s = "LevelStars" + data.LevelIndex.ToString();
            PlayerPrefs.SetInt(s, data.StarEarned);
        }
    }

    public void UpdateStars()
    {
        levelData.levelStarData[MapTransition.instance.CurrentLevel - 1].StarCount(UIController.instance.StepCount);
    }

    public void CheckWin()
    {
        if (levelComplete || CurrentState == GameState.restart)
            return;

        bool win = true;
        foreach (var tile in ChangableTiles)
        {
            if (tile.GetComponent<TileManager>().MapColor == TileColor.red)
            {
                win = false;
                break;
            }
        }

        if (win)
        {
            WinGame();
        }
        else
        {
            //Debug.Log("Not yet");
        }
    }

    public bool IsLevelCompleted()
    {
        return levelComplete;
    }

    public void resetAnalyticsTimer(int levelIndex)
    {
        levelComplete = false;
        startTime = Time.time;
        totalPauseDuration = 0;
        currentLevel = levelIndex;
    }
    //"[Analytics] Completed level:
    public void SendLevelAnalytics(string levelEvent)
    {
        int steps = UIController.instance.GetStep();
        float duration = Time.time - startTime - totalPauseDuration;
        duration = Mathf.Round(duration * 100.0f) * 0.01f;
        Debug.Log("[Analytics]  " + levelEvent + " level " + currentLevel + " in " + steps + " steps. Duration: " + duration + "s");

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("level", currentLevel);
        parameters.Add("steps", steps);
        parameters.Add("duration", duration);

        // Event too big, disable this for now

        /*
        Dictionary<string, object> heatMapData = new Dictionary<string, object>();
        heatMapData.Add("level", currentLevel);
        CollectHeatMapData(heatMapData);
        */
        

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult analyticsResult = Analytics.CustomEvent(levelEvent, parameters);
        Debug.Log("[Analytics] " + levelEvent + " Sent: " + analyticsResult);
        /*
        analyticsResult = Analytics.CustomEvent("heatMapData", heatMapData);
        Debug.Log("[Analytics] " + heatMapData.Count + " heatMapData Sent: " + analyticsResult);
        */
#endif
    }

    public void CollectHeatMapData(Dictionary<string, object> heatMap)
    {
        List<Node> allNodes = Graph.instance.GetAllNodes();

        int nodeId = 0;
        foreach (Node node in allNodes)
        {
            string nodeName = "Node" + nodeId;
            HeatMapData data = new HeatMapData(node, nodeId);
            string json = JsonUtility.ToJson(data);
            /*nodes.Add(nodeName, json);
            Debug.Log("[Analytics]" + nodeName + " " + json);*/
            heatMap.Add(nodeName, json);
            nodeId++;
        }

    }

    private void OnApplicationQuit()
    {
        foreach (levelStar ls in levelData.levelStarData)
        {
            ls.StarEarned = 0;
        }
        
    }

}
