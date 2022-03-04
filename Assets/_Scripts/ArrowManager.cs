using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrowManager : MonoBehaviour
{
    public static ArrowManager instance;
    [SerializeField]
    private List<GameObject> arrows;
    [SerializeField]
    private List<string> hints;
    [SerializeField]
    private TMP_Text UiText;
    public int arrowsCount = 0;
    public int arrowsTrack = 0;
    
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        arrowsCount = arrows.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextArrow()
    {
        arrows[arrowsTrack].SetActive(false);
        UiText.text = "";
        if ((arrowsTrack+1) < arrowsCount)
        {
            arrowsTrack++;
            arrows[arrowsTrack].SetActive(true);
            UiText.text = hints[arrowsTrack];
        }

    }
}
