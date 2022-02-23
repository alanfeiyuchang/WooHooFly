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
    public GameState CurrentState = GameState.starting;
    public float startTime;
    public float totalPauseDuration;
    public int currentLevel = 1;
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

        // disable player controller
        MapTransition.instance.GetCurrentCubeControllerScript().enabled = false;

        // Get level duration
        float duration = Time.time - startTime - totalPauseDuration;
        duration = Mathf.Round(duration * 100.0f) * 0.01f;
        Debug.Log("Time taken: " + duration + " seconds.");

        // Send analytics data on winnning the level
        SendLevelCompleteAnalytics();
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
        currentLevel = levelIndex + 1;
    }

    public void SendLevelCompleteAnalytics()
    {
        int steps = UIController.instance.GetStep();
        float duration = Time.time - startTime - totalPauseDuration;
        duration = Mathf.Round(duration * 100.0f) * 0.01f;
        Debug.Log("[Analytics] Completed level: " + currentLevel + " in " + steps + " steps. Duration: " + duration + "s");

#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult analyticsResult = Analytics.CustomEvent("levelComplete", new Dictionary<string, object>
        {
            { "level", currentLevel },
            { "steps", steps },
            { "duration" , duration }
        });
        Debug.Log("[Analytics] Sent: " + analyticsResult);
#endif
    }

    public void SendRestartAnalytics()
    {
        int steps = UIController.instance.GetStep();
        float duration = Time.time - startTime - totalPauseDuration;
        duration = Mathf.Round(duration * 100.0f) * 0.01f;
        Debug.Log("[Analytics] Restarted level: " + currentLevel + " after " + steps + " steps. Duration: " + duration + "s");
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult analyticsResult = Analytics.CustomEvent("levelRestart", new Dictionary<string, object>
        {
            { "level", currentLevel },
            { "steps", steps },
            { "duration" , duration }
        });
        Debug.Log("[Analytics] Sent: " + analyticsResult);
#endif
    }
}
