using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
// using WooHooFly.Tutorial;
[System.Serializable]
public class TutorialHint
{
    public GameObject HintPlace;
    public UnityEvent HintEvent;
}

// [System.Serializable]
// public class HintEvent : UnityEvent<string> {}


public class TutorialManager : MonoBehaviour
{
    public static TutorialManager current;

    private GameObject _text;
    [SerializeField] 
    private GameObject _path;

    public TutorialHint[] tutorialHints;

    public GameObject Arrow;
    private GameObject _arrow;

    private bool isPlaying;

    private void Awake()
    {
        current = this;
        // Initiate TutorialManager event listeners
        GameManager.instance.onGameStateChanged += GameStateChanged;
        // TutorialManager.current.onHighLightPosEnter +=  HighlightPath;
        // TutorialManager.current.onArrowHit += NextArrowPosition;
        // TutorialManager.current.onTextHit += NextTextShow;
        _text = transform.GetChild(0).gameObject;

        if (tutorialHints != null) {
            foreach (TutorialHint tutorialHint in tutorialHints)
            {
                // Debug.Log(tutorialHint.HintPlace.name);
                HintTrigger hintTrigger = tutorialHint.HintPlace.transform
                                            .GetChild(0).GetChild(0).gameObject
                                            .AddComponent<HintTrigger>();
                hintTrigger.HintEvent = tutorialHint.HintEvent;
            }
        }
    }

    private void Start()
    {

    }

    public void ShowTextHint(String i) {
        _text.GetComponent<Text>().text = i;
    }

    public void ShowArrowHint(GameObject i) {
        NextArrowPosition(i);
    }



    private void OnDestroy() {
        GameManager.instance.onGameStateChanged -= GameStateChanged;
        // TutorialManager.current.onHighLightPosEnter -=  HighlightPath;
        // TutorialManager.current.onArrowHit -= NextArrowPosition;
        // TutorialManager.current.onTextHit -= NextTextShow;
    }

    // private void OnDisable() {
    //      Debug.Log("DISABLED");
    // }

    public void GameStateChanged(GameManager.GameState state) 
    {
        if (state == GameManager.GameState.starting || state == GameManager.GameState.restart) {

            if (!isPlaying){
                ShowTutorialText();
            }
        }
        else if (state != GameManager.GameState.playing && state != GameManager.GameState.rotating) {
            StopTutorialText();
        }

        if (state == GameManager.GameState.win) {
            StopArrow();
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

    public void HighlightPath() {
        Debug.Log("highting-----");
        _path.SetActive(true);
        StartCoroutine(FadePath());
    }


    IEnumerator FadePath() {
        int glowCount = 5;
        while (glowCount-- > 0) {
            MeshRenderer[] glows = _path.GetComponentsInChildren<MeshRenderer>();
            
            yield return StartCoroutine(FadePathToFullAlpha(1f, glows ));
            yield return new WaitForSeconds(1.0f);
            yield return StartCoroutine(FadePathToZeroAlpha(1f, glows ));   
            
            
        }
         
    }

    public IEnumerator FadePathToZeroAlpha(float t, MeshRenderer[] glows)
    {
        foreach (MeshRenderer glow in glows)
        {
            Material i = glow.material;
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        }
        
        float alpha  = glows[0].material.color.a ;

        while (alpha > 0.0f)
        {
            alpha -= (Time.deltaTime / t);
            foreach (MeshRenderer glow in glows)
            {
                Material i = glow.material;
                i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
            }         
            yield return null;
        }
    }

    public IEnumerator FadePathToFullAlpha(float t, MeshRenderer[] glows)
    {
        foreach (MeshRenderer glow in glows)
        {
            Material i = glow.material;
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        }
        
        float alpha = glows[0].material.color.a ;
        while (alpha < 1.0f)
        {
            alpha += (Time.deltaTime / t);
            foreach (MeshRenderer glow in glows)
            {
                Material i = glow.material;
                i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
            }         
            yield return null;
        }
    }

    // Show Arrow on Mapcube
    private void NextArrowPosition(GameObject mapCube){
        Debug.Log("change to next postion");
        if (_arrow != null)
            Destroy(_arrow);

        GameObject arrow = Instantiate(Arrow, 
                    new Vector3(0, 0, 0),
                    Quaternion.Euler(new Vector3(0, 150, 90)));
        
        arrow.transform.parent = mapCube.transform;
        _arrow = arrow; 

    }

    private void StopArrow() {
        if (_arrow != null)
            Destroy(_arrow);
    }


}