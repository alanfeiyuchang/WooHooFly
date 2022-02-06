using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIController : MonoBehaviour
{
    public static UIController instance;
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
    }

    private void CloseMenu()
    {
        WinPanel.SetActive(false);
    }

    public void WinUI()
    {
        WinPanel.SetActive(true);
    }

    public void RestartButtonPressed()
    {
        CloseMenu();
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddStep()
    {
        StepCount++;
    }
}
