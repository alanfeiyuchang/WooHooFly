using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Collections;
using System.Collections.Generic;
using WooHooFly.Colors;
using WooHooFly.NodeSystem;

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

    public List<GameObject> checkPath;

    public TutorialHint[] tutorialHints;

    public GameObject Arrow;
    public GameObject SideArrow;
    private GameObject _arrow;
    private bool isSide;

    private bool activatedText, activatedArrow;

    private bool isPlaying;

    private string[] directions = {"R2DL", "R2UL", "L2UR", "L2DR"} ;
    private int index = 2;

    private List<Clickable> clickables = new List<Clickable>();

    private void Awake()
    {
        //Debug.Log("TutorialManager Awake");
        current = this;
        // Initiate TutorialManager event listeners
        GameManager.instance.onGameStateChanged += GameStateChanged;
        // TutorialManager.current.onHighLightPosEnter +=  HighlightPath;
        // TutorialManager.current.onArrowHit += NextArrowPosition;
        // TutorialManager.current.onTextHit += NextTextShow;
        _text = transform.GetChild(0).gameObject;

        if (tutorialHints != null)
        {
            Debug.Log("tutorial hints: " + this.transform.parent.name + " " + tutorialHints.Length);
            foreach (TutorialHint tutorialHint in tutorialHints)
            {
                BoxCollider bc = new BoxCollider();
                HintTrigger hintTrigger = new HintTrigger();
                if (tutorialHint.HintPlace.name.StartsWith("Side")) {
                    bc = tutorialHint.HintPlace.transform.GetChild(0).gameObject.AddComponent<BoxCollider>();
                    hintTrigger = tutorialHint.HintPlace.transform
                                            .GetChild(0).gameObject
                                            .AddComponent<HintTrigger>();
                    bc.isTrigger = true;
                    bc.size = new Vector3(0.2f, 0.2f, 0.05f);
                    bc.center = new Vector3(0, 0, 0);
                    hintTrigger.HintEvent = tutorialHint.HintEvent;
                }
                else if  (tutorialHint.HintPlace.name.StartsWith("MapCube"))  {
                    bc = tutorialHint.HintPlace.transform.GetChild(0).GetChild(0).gameObject.AddComponent<BoxCollider>();
                    hintTrigger = tutorialHint.HintPlace.transform
                                            .GetChild(0).GetChild(0).gameObject
                                            .AddComponent<HintTrigger>();
                    bc.isTrigger = true;
                    bc.size = new Vector3(0.2f, 0.2f, 0.05f);
                    bc.center = new Vector3(0, 0, 0);
                    hintTrigger.HintEvent = tutorialHint.HintEvent;
                }


                // Debug.Log(tutorialHint.HintPlace.name);
                if (tutorialHint.HintPlace.name == "Coloring_Tile") {
                    clickables.Add(tutorialHint.HintPlace.GetComponentInChildren<Clickable>());
                }
                  
               
                foreach (Clickable c in clickables)
                {
                    c.clickAction += OnClickTile;
                    c.hintEvent = tutorialHint.HintEvent;
                }
                
               
            }
        }
        else
        {
            Debug.Log("No tutorial hints");
        }
    }

    private void Start()
    {

    }

    public void OnClickTile(Clickable clickable, Vector3 position) {
        clickable.loadTutorial();
    }

    public void ShowTextHint(String i) {
        _text.GetComponent<Text>().text = i;
    }

    public void ShowArrowHint(GameObject i) {
        NextArrowPosition(i);
    }



    private void OnDestroy() {
        GameManager.instance.onGameStateChanged -= GameStateChanged;
        foreach (Clickable c in clickables)
        {
            c.clickAction -= OnClickTile;
        }
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
        if (_path != null) {
            _path.SetActive(true);
            StartCoroutine(FadePath());
        }
        
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
        // Debug.Log("change to next postion");
        if (_arrow != null)
            Destroy(_arrow);

        // Debug.Log("initiate " + mapCube.name + " of " + mapCube.transform.parent.name);
        // GameObject arrow = Instantiate(Arrow, 
        //             new Vector3(0, 0, 0),
        //             Quaternion.Euler(new Vector3(0, 150, 90)));
        GameObject arrow;
        if (mapCube.name == "Side C") {
            isSide = true;
            arrow = Instantiate(SideArrow);
            arrow.transform.localRotation = SideArrow.transform.localRotation;
            GameObject arrow_container = new GameObject("arrow");
        
            arrow_container.transform.parent = mapCube.transform;
            // Vector3 original = mapCube.transform.position;
            arrow_container.transform.localPosition = new Vector3(0,  1.5f, 0 );
            arrow_container.transform.localEulerAngles = new Vector3(0, -90, -90);
            arrow.transform.parent = arrow_container.transform;
            ArrowController ac = arrow_container.GetComponentInChildren<ArrowController>();
            ac.OnRotateMap(directions[index]);
             _arrow = arrow_container; 
        }
        else if (mapCube.name == "Side A" || mapCube.name.StartsWith("MapCube")) {
            isSide = false;
            arrow = Instantiate(Arrow);
            arrow.transform.localRotation = Arrow.transform.localRotation;
            GameObject arrow_container = new GameObject("arrow");
        
            arrow_container.transform.parent = mapCube.transform;
            // Vector3 original = mapCube.transform.position;
            arrow_container.transform.localPosition = new Vector3(0,  1.5f, 0 );
            arrow.transform.parent = arrow_container.transform;
             _arrow = arrow_container; 
        }
        
        // ac.vector = mapCube.transform.position;
        // arrow_container.transform.LookAt(cam.transform.position);

       
    }

    public void DeactiveArrow() {
        if (_arrow != null)
            _arrow.SetActive(false);
            
    }

    public void DestroyArrow() {
        if (_arrow != null)
            Destroy(_arrow);
            
    }

    public void ShowTextHintOnce(string text) {
        if (!activatedText) {
            ShowTextHint(text);
            activatedText = true;
        }
    }

    public void ShowArrowHintOnce(GameObject mapCube) {
        if (!activatedArrow) {
            ShowArrowHint(mapCube);
            activatedArrow = true;
        }
    }

    public void RotateArrowOnRotation(float angle) {
         if (_arrow != null && !isSide ) {
             Debug.Log("Original arrow rotate");
            Vector3 original = _arrow.transform.localEulerAngles;
            _arrow.transform.localEulerAngles = new Vector3(original.x, original.y-angle, original.z); 
            _arrow.SetActive(true);
         }
        
    }

    public void RotateSideArrow(float angle) {
        
        if (angle > 0) {
                index = (index + 1) % directions.Length;
                
        }
        else {
            if (index == 0)
                index = directions.Length - 1;
            else
                index = index - 1;
        }
        if (_arrow != null && isSide) {
            Debug.Log("new arrow rotate");
             ArrowController ac = _arrow.GetComponentInChildren<ArrowController>();
            ac.OnRotateMap(directions[index]);
            _arrow.SetActive(true);
         }
        
    }

    private void StopArrow() {
        if (_arrow != null)
            Destroy(_arrow);
    }

    public void ShowArrowOnMissedCube() {
        List<GameObject> notGreens = new List<GameObject>();

        foreach (GameObject checkCube in checkPath)
        {
            TileManager tileManager = checkCube.GetComponentInChildren<TileManager>();
            TileColor color = tileManager.MapColor;
            if (color != TileColor.green) {
                notGreens.Add(checkCube);
            }
        }
        checkPath = notGreens;

        foreach (GameObject checkCube in notGreens)
        {
            NextArrowPosition(checkCube);
            ShowTextHint("You missed a tile!");
            return;
        }      

   

    }

}