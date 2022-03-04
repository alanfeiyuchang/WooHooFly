using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager current;

    private GameObject _text;
    [SerializeField] 
    private GameObject _path;

    public List<string> TutorialTexts = new List<string>();
    private Queue<string> _TutorialTextsQueue;

    public List<Transform> ArrowShowUpPosition;
    private Queue<Transform> _ArrowShowUpPositionQueue;

    public GameObject Arrow;
    private GameObject _arrow;

    private bool isPlaying;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        // Initiate TutorialManager event listeners
        GameManager.instance.onGameStateChanged += StartTutorial;
        TutorialManager.current.onHighLightPosEnter +=  HighlightPath;
        TutorialManager.current.onArrowHit += NextArrowPosition;
        _text = transform.GetChild(0).gameObject;
        // Transform[] c = gameObject.GetComponentsInChildren<Transform>(true);
        // foreach (Transform child in c)
        // {
        //     if (child.gameObject.name == "Path")
        //         _path = child.gameObject;
        // }
        Debug.Log("STARTED" + (_path == null));

        if (ArrowShowUpPosition != null) {
            _ArrowShowUpPositionQueue = new Queue<Transform>(ArrowShowUpPosition);
            _TutorialTextsQueue = new Queue<string>(TutorialTexts);
            NextArrowPosition();
        }
    }


    private void OnDestroy() {
        Debug.Log("DESTROYED");
        GameManager.instance.onGameStateChanged -= StartTutorial;
        TutorialManager.current.onHighLightPosEnter -=  HighlightPath;
        TutorialManager.current.onArrowHit -= NextArrowPosition;
    }

    private void OnDisable() {
         Debug.Log("DISABLED");
    }


    public event Action onHighLightPosEnter;
    public event Action onArrowHit;

    public void HighLightPosEnter() {
        if (onHighLightPosEnter != null) {
            onHighLightPosEnter();
        }
    }
    public void ArrowHit() {
        if (onArrowHit != null) {
            onArrowHit();
        }
    }

    public void StartTutorial(GameManager.GameState state) 
    {

        if (state == GameManager.GameState.starting) {

            if (!isPlaying){
                ShowTutorialText();
            }
        }
        else if (state != GameManager.GameState.playing && state != GameManager.GameState.rotating) {
            StopTutorialText();
        }

    }

    private void ShowTutorialText()
    {
        isPlaying = true;
        _text.SetActive(true);
    }

    private void StopTutorialText()
    {
        isPlaying = false;
        _text.SetActive(false);
    }

    private void NextTextShow()
    {
        Debug.Log("next text");
        if (_TutorialTextsQueue.Count == 0)
            return;
        _text.GetComponent<Text>().text = _TutorialTextsQueue.Dequeue();
    }

    public void HighlightPath() {
        Debug.Log("highting-----" + _path.name + " " + _path.activeInHierarchy);
        _path.SetActive(true);
    }


    IEnumerator FadePath() {
        yield return new WaitForSeconds(5.0f);
        // yield return StartCoroutine(FadePathToZeroAlpha(1f, _text.GetComponent<Text>()));
    }

    public IEnumerator FadePathToZeroAlpha(float t, Material i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }

    public void NextArrowPosition(){
        Debug.Log("change to next postion");
        if (_arrow != null)
            Destroy(_arrow);
        if (_ArrowShowUpPositionQueue.Count == 0)
            return;

        GameObject arrow = Instantiate(Arrow, 
                    new Vector3(0, 0, 0),
                    Quaternion.Euler(new Vector3(0, 150, 90)));
        
        arrow.transform.parent = _ArrowShowUpPositionQueue.Dequeue();
        _arrow = arrow; 

        NextTextShow();       
    }

    //  // ----- Tutotial UI old methods ------
    // private void ShowTutorialText()
    // {
    //     Debug.Log("tutorial starts");
    //     isPlaying = true;
    //     _text.SetActive(true);
       
    //     StartCoroutine(FadeInTextEffect());
    // }

    // IEnumerator FadeInTextEffect()
    // {   
    //     int index = 0;
    //     while (isPlaying)
    //     { 
    //         yield return StartCoroutine(FadeTextToFullAlpha(1f, _text.GetComponent<Text>(),TutorialTexts[index++]));
    //         if (index == TutorialTexts.Count)
    //             index = 0;

    //         yield return new WaitForSeconds(2.0f);
    //         yield return StartCoroutine(FadeTextToZeroAlpha(1f, _text.GetComponent<Text>()));
            
    //     }
    //    Debug.Log("tutorial stops");
    // }

    // private void StopTutorial()
    // {
    //     isPlaying = false;
    //     _text.SetActive(false);
    // }




    // public IEnumerator FadeTextToFullAlpha(float t, Text i, String text)
    // {
        
    //     i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
    //     i.text = text;
    //     while (i.color.a < 1.0f)
    //     {
    //         i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
    //         yield return null;
    //     }
    // }
 
    // public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    // {
    //     i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
    //     while (i.color.a > 0.0f)
    //     {
    //         i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
    //         yield return null;
    //     }
    // }

}