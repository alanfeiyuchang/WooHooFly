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
    private bool stepCounterActive = false;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject StartMenu;
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject PauseMene;
    [SerializeField] private GameObject InGamePanel;
    [SerializeField] private CubeControllerNew CubeControllerNewScript;
    [SerializeField] private MouseRotation MouseRotationScript;


    [SerializeField] private TMP_Text StepCountText;
    private int _stepCount = 0;
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
        DisableController();

        // Reset UI
        CloseMenu();
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
        StartMenu.SetActive(false);
        WinPanel.SetActive(false);
        PauseMene.SetActive(false);
    }

    private void DisableController()
    {
        CubeControllerNewScript.enabled = false;
        MouseRotationScript.enabled = false;
    }

    private void EnableController()
    {
        CubeControllerNewScript.enabled = true;
        MouseRotationScript.enabled = true;
    }

    public void WinUI()
    {
        WinPanel.SetActive(true);

        // disable step counter
        stepCounterActive = false;
        StepCount++;
        // Send analytic event for steps
    }

    public void StartButtonPressed()
    {
        CloseMenu();
        InGamePanel.SetActive(true);
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        EnableController();
    }

    public void RestartButtonPressed()
    {
        CloseMenu();
        GameManager.instance.CurrentState = GameManager.GameState.restart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reload scene");
    }

    public void PauseMenuClicked()
    {
        if (WinPanel.activeInHierarchy)
            return;

        if (PauseMene.activeInHierarchy)
        {
            PauseMene.SetActive(false);
            GameManager.instance.CurrentState = GameManager.GameState.playing;
        }
        else
        {
            PauseMene.SetActive(true);
            GameManager.instance.CurrentState = GameManager.GameState.paused;
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
