using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class UIController : MonoBehaviour
{
    public static UIController instance;
    [HideInInspector]
    public bool stepCounterActive = false;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject PauseMene;
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private TMP_Text StepCountText;
    [SerializeField] private Animation leftAnim;
    [SerializeField] private Animation rightAnim;
    [SerializeField] private GameObject cwArrow;
    [SerializeField] private GameObject ccwArrow;
    public GameObject NextButton;
    private int _stepCount = 0;
    private float pauseStart;
    private float pauseDuration;
    public int StepCount
    {
        get => _stepCount;
        set
        {
            _stepCount = value;
            StepCountText.text = "Steps: " + _stepCount.ToString();
        }
    }

    private void Start()
    {
        // Disable user control
        //MapTransition.instance.DisableController();

        // Reset UI
        CloseMenu();
        //Title.SetActive(true);
        //StartMenu.SetActive(true);
        InGamePanel.SetActive(true);

        // Enable step counter
        stepCounterActive = true;

        // Analytics data
        SendStartAnalytics();


    }

private void SendStartAnalytics()
    {
        Debug.Log("[Analytics] Level "+ GameManager.instance.currentLevel + " started");
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult analyticsResult = Analytics.CustomEvent("newGame", new Dictionary<string, object>
        {
            { "level", GameManager.instance.currentLevel }
        });
        Debug.Log("[Analytics] sent: " + analyticsResult);
#endif
    }
    

    private void CloseMenu()
    {
        Title.SetActive(false);
        StartMenu.SetActive(false);
        WinPanel.SetActive(false);
        PauseMene.SetActive(false);
    }

    
    public void WinUI()
    {
        WinPanel.SetActive(true);
        
        // disable step counter
        stepCounterActive = false;
        // Send analytic event for steps

        // //End Tutorial if available
        // if (TutorialManager.current != null) {
        //     TutorialManager.current.EndPositionEnter();
        // }
    }

    public void StartButtonPressed()
    {
        CloseMenu();
        InGamePanel.SetActive(true);
        
        //MapTransition.instance.LevelTransition();
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        MapTransition.instance.EnableController();
        GameManager.instance.startTime = Time.time;

        // //Show Tutorial if available
        // if (TutorialManager.current != null) {
        //     TutorialManager.current.StartPositionEnter();
        // }
    }

    public void HomeButtonPressed()
    {
        CloseMenu();
        InGamePanel.SetActive(true);
        MapTransition.instance.SelectLevel();
    }

    public void NextButtonPressed()
    {
        CloseMenu();
        MapTransition.instance.LevelTransition();
    }
    public void RestartButtonPressed()
    {
        CloseMenu();
        stepCounterActive = true;

        // if player did not complete the level, record analytics data
        if ( ! GameManager.instance.IsLevelCompleted() )
        {
            GameManager.instance.SendRestartAnalytics();
        }
        
        GameManager.instance.CurrentState = GameManager.GameState.restart;
        MapTransition.instance.RestartLevel();
    }

    public void PauseMenuClicked()
    {
        if (WinPanel.activeInHierarchy)
            return;

        if (PauseMene.activeInHierarchy)
        {
            PauseMene.SetActive(false);
            InGamePanel.SetActive(true);
            GameManager.instance.CurrentState = GameManager.GameState.starting;
            pauseDuration = Time.time - pauseStart;
            GameManager.instance.totalPauseDuration += pauseDuration;
        }
        else
        {
            PauseMene.SetActive(true);
            InGamePanel.SetActive(false);
            GameManager.instance.CurrentState = GameManager.GameState.paused;
            pauseStart = Time.time;
        }
    }

    public void AddStep()
    {
        if (stepCounterActive)
        {
            StepCount++;
        }
        
    }

    public int GetStep()
    {
        return StepCount;
    }

    public void RotateCW()
    {
        if (MapTransition.instance.mouseRotation != null)
        {
            Debug.Log(MapTransition.instance.mouseRotation.name);
            MapTransition.instance.mouseRotation.RotateMapCW();
        }
    }

    public void RotateCCW()
    {
        if (MapTransition.instance.mouseRotation != null)
        {
            Debug.Log(MapTransition.instance.mouseRotation.name);
            MapTransition.instance.mouseRotation.RotateMapCCW();
        }
    }

    public void StartArrowAnim()
    {
        leftAnim.Play();
        rightAnim.Play();
    }
   
    public void StopArrowAnim()
    {
        leftAnim.Stop();
        rightAnim.Stop();
    }
    public void HideRotateArrow()
    {
        cwArrow.SetActive(false);
        ccwArrow.SetActive(false);
    }
    public void ShowRotateArrow()
    {
        cwArrow.SetActive(true);
        ccwArrow.SetActive(true);
    }
    // add more methods to track other stats
}
