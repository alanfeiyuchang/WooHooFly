using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    public static UIController instance;
    private bool stepCounterActive = false;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject WinPanel;

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
        WinPanel.SetActive(false);

        //enable step counter
        stepCounterActive = true;
        Debug.Log("New Game");
    }

    private void CloseMenu()
    {
        WinPanel.SetActive(false);
    }

    public void WinUI()
    {
        WinPanel.SetActive(true);

        // disable step counter
        stepCounterActive = false;
        StepCount++;
        // Send analytic event for steps
    }

    public void RestartButtonPressed()
    {
        CloseMenu();
        //GameManager.instance.CurrentState = GameManager.GameState.restart;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Reload scene");
    }

    public void AddStep()
    {
        if (stepCounterActive)
        {
            StepCount++;
        }
        
    }

    // add more methods to track other stats
}
