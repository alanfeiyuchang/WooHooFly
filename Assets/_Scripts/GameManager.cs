using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;
using WooHooFly.NodeSystem;
using TileSystem;

#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public AudioClip[] music;
    private AudioSource BGM;

    private bool isFinal; // is final map

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
    public PathFinder _pathFinder;
    //----- end ----

    // Analytics variables
    public float startTime;
    public float totalPauseDuration;
    private int currentLevel = 1;
    public LevelData levelData;
    private bool levelComplete = false;

    [HideInInspector]
    public Material SealedMaterial;
    
    [Header("Fire material")]
    [SerializeField] private Material _fireMaterial;
    [SerializeField] private Color _blue;
    [SerializeField] private Color _red;
    [SerializeField] private float _changeFrequency = 2f;
    [SerializeField] private float _colorChangeGap = 1f;
    private bool _changingColor = false;

    //public Direction levelDirection = Direction.None;
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

    private void Update()
    {
        /*if (!_changingColor)
        {
            StartCoroutine(ColorChange());
        }*/
    }

    IEnumerator ColorChange()
    {
        _changingColor = true;
        float timeElapsed = 0f;
        while (timeElapsed <= _changeFrequency)
        {
            timeElapsed += Time.deltaTime;
            Color col = Color.Lerp(_blue, Color.white, timeElapsed / _changeFrequency);
            col *= 16f;
            _fireMaterial.SetColor("_Color", col);
            yield return null;
        }
        timeElapsed = 0f;
        while (timeElapsed <= _changeFrequency)
        {
            timeElapsed += Time.deltaTime;
            Color col = Color.Lerp(Color.white, _red, timeElapsed / _changeFrequency);
            col *= 16f;
            _fireMaterial.SetColor("_Color", col);
            yield return null;
        }
        timeElapsed = 0f;

        yield return new WaitForSeconds(_colorChangeGap);

        while (timeElapsed <= _changeFrequency)
        {
            timeElapsed += Time.deltaTime;
            Color col = Color.Lerp(_red, Color.white, timeElapsed / _changeFrequency);
            col *= 16f;
            _fireMaterial.SetColor("_Color", col);
            yield return null;
        }
        timeElapsed = 0f;
        while (timeElapsed <= _changeFrequency)
        {
            timeElapsed += Time.deltaTime;
            Color col = Color.Lerp(Color.white, _blue, timeElapsed / _changeFrequency);
            col *= 16f;
            _fireMaterial.SetColor("_Color", col);
            yield return null;
        }
        timeElapsed = 0f;

        yield return new WaitForSeconds(_colorChangeGap);
        _changingColor = false;
    }

    public void WinGame()
    {
        BGM = this.GetComponent<AudioSource>();
        BGM.clip = music[1];
        BGM.Play();
        CurrentState = GameState.win;
        Debug.Log("***************WON***************");
        if (!isFinal)
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
        if (MapTransition.instance.CurrentLevel != 0)
            levelData.levelStarData[MapTransition.instance.CurrentLevel - 1].StarCount(UIController.instance.StepCount);
    }

    public bool CheckWin(GameObject finalMap)
    {
        LevelManager currentLevelManager = MapTransition.instance.GetCurrentLevel();

        if (_pathFinder.FindPath(currentLevelManager.StartNode, currentLevelManager.FinishNode).Count == 0)
        {
            Debug.Log("Not win");
            return false;
        }
        else
        {
            Debug.Log("Win!! " + _pathFinder.FindPath(currentLevelManager.StartNode, currentLevelManager.FinishNode).Count.ToString());
            StartCoroutine(Transit(finalMap, currentLevelManager));
            return true;
        }
    }

    IEnumerator Transit(GameObject finalMap, LevelManager currentLevelManager)
    {
        if (currentLevelManager.NeedAutomaticRotation)
        {
            float currentAngle = (int)currentLevelManager.transform.localEulerAngles.y;
            float targetAngle = (int)currentLevelManager.FinalRotationAngle;
            if (currentAngle != targetAngle)
            {
                GameManager.instance.CurrentState = GameState.playing;
                float angleDiff = Mathf.DeltaAngle(currentAngle, targetAngle);
                Debug.Log("Angle Diff " + angleDiff);
                Debug.Log(angleDiff + "  " + currentLevelManager.GetComponent<MouseRotation>() != null);
                if (angleDiff == -90 && currentLevelManager.GetComponent<MouseRotation>() != null)
                {
                    currentLevelManager.GetComponent<MouseRotation>().RotateMapCCW();
                }
                else if (angleDiff == 90 && currentLevelManager.GetComponent<MouseRotation>() != null)
                {
                    currentLevelManager.GetComponent<MouseRotation>().RotateMapCW();
                }
                else if ((angleDiff == 180 || angleDiff == -180) && currentLevelManager.GetComponent<MouseRotation>() != null)
                {
                    currentLevelManager.GetComponent<MouseRotation>().RotateTwoTimes();
                }
                yield return new WaitForSeconds(1.5f);
            }
        }
        if (finalMap != null)
            isFinal = true;
        WinGame();
        if (finalMap != null)
        {
            finalMap.GetComponent<FinalTransition>().Begin();
        }
        else
        {
            if (DissolveTransition.instance != null)
            {
                DissolveTransition.instance.spawnGround();
                DissolveTransition.instance.startDissolve(2f);
            }
            if (RiverGenerator.instance != null)
            {
                RiverGenerator.instance.GenerateRealWorld();
            }
        }
        if (SealedMaterial != null)
        {
            Color temp = SealedMaterial.color;
            temp.a = 0f;
            SealedMaterial.color = temp;
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

        if (SealedMaterial != null)
        {
            Color temp = SealedMaterial.color;
            temp.a = 0.6f;
            SealedMaterial.color = temp;
        }
    }

}
