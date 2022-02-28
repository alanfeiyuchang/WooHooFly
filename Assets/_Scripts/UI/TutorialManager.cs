using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager current;

    public List<string> TutorialTexts = new List<string>();
    private GameObject _text;
    // private GameObject _goal;

    private bool isPlaying;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        // Initiate TutorialManager event listeners
        TutorialManager.current.onStartPositionEnter +=  ShowTutorialText;
        TutorialManager.current.onEndPositionEnter += StopTutorial;
        _text = transform.GetChild(0).gameObject;
        // _goal = transform.GetChild(1).gameObject;
    }


    public event Action onStartPositionEnter;
    public event Action onEndPositionEnter;

    public void StartPositionEnter() {
        if (onStartPositionEnter != null) {
            onStartPositionEnter();
        }
    }

    public void EndPositionEnter() {
        if (onEndPositionEnter != null) {
            onEndPositionEnter();
        }
    }

    // ----- Tutotial UI methods ------
    private void ShowTutorialText()
    {
        isPlaying = true;
        _text.SetActive(true);
        StartCoroutine(ShowTutorialGoal());
    }

    IEnumerator ShowTutorialGoal()
    {   
        int index = 0;
        while (isPlaying)
        { 
            yield return StartCoroutine(FadeTextToFullAlpha(1f, _text.GetComponent<Text>(),TutorialTexts[index++]));

            if (index == TutorialTexts.Count)
                index = 0;

            yield return new WaitForSeconds(3.0f);
            yield return StartCoroutine(FadeTextToZeroAlpha(1f, _text.GetComponent<Text>()));
        }
       
        // _goal.SetActive(true);
    }

    private void StopTutorial()
    {
        isPlaying = false;
        _text.SetActive(false);
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i, String text)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        i.text = text;
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

}