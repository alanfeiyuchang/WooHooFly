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
        MapTransition.instance.DisableController();

        // Reset UI
        CloseMenu();
        Title.SetActive(true);
        StartMenu.SetActive(true);
        InGamePanel.SetActive(false);

        // Enable step counter
        stepCounterActive = true;

        // Analytics data
        Debug.Log("New Game");
#if ENABLE_CLOUD_SERVICES_ANALYTICS
        AnalyticsResult analyticsResult = Analytics.CustomEvent("newGame", new Dictionary<string, object>
        {
            { "level", 1 }
        });
        Debug.Log("Analytics data sent: " + analyticsResult);
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
    }

    public void StartButtonPressed()
    {
        CloseMenu();
        InGamePanel.SetActive(true);
        
        //MapTransition.instance.LevelTransition();
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        MapTransition.instance.EnableController();
        GameManager.instance.startTime = Time.time;
    }

    public void RestartButtonPressed()
    {
        CloseMenu();
        stepCounterActive = true;
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
            GameManager.instance.CurrentState = GameManager.GameState.playing;
            pauseDuration = Time.time - pauseStart;
            GameManager.instance.totalPauseDuration += pauseDuration;
        }
        else
        {
            PauseMene.SetActive(true);
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

    // add more methods to track other stats
}
