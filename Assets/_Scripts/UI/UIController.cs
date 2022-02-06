using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIController : MonoBehaviour
{
    public static UIController instance;
    private void Awake()
    {
        instance = this;
    }
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

    public void AddStep()
    {
        StepCount++;
    }
}
