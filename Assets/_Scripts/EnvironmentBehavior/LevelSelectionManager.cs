using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager instance;
    private void Awake()
    {
        instance = this;
    }

    public List<GameObject> LevelTriggerBoxes;
    public List<GameObject> LevelStars;

    private void Start()
    {
        LoadLevelSelection();
        UpdateLevelSelection();
    }

    public void LoadLevelSelection()
    {
        if (PlayerPrefs.HasKey("LevelUnlocked"))
            MapTransition.instance.LevelUnlocked = PlayerPrefs.GetInt("LevelUnlocked");
        GameManager.instance.LoadPlayerPrefsStars();
        
        
    }

    public void UpdateLevelSelection()
    {
        GameManager.instance.UploadPlayerPrefsStars();
        PlayerPrefs.SetInt("LevelUnlocked", MapTransition.instance.LevelUnlocked);
        int index = 1;
        foreach (GameObject star in LevelStars)
        {
            string s = "LevelStars" + index.ToString();
            if (PlayerPrefs.HasKey(s) && star.GetComponent<StarManager>() != null)
            {
                star.GetComponent<StarManager>().changeStar(PlayerPrefs.GetInt(s));
            }
            else
            {
                PlayerPrefs.SetInt(s, 0);
                star.GetComponent<StarManager>().changeStar(0);
            }
            index++;
        }
        for (int i = 0; i < MapTransition.instance.LevelUnlocked; i++)
        {
            if (i >= LevelTriggerBoxes.Count)
                break;
            Debug.Log(LevelTriggerBoxes[i].GetComponent<MapCubeManager>().name);
            LevelTriggerBoxes[i].GetComponent<MapCubeManager>().sideAColor = WooHooFly.Colors.TileColor.green;
            LevelTriggerBoxes[i].GetComponent<MapCubeManager>().
                changeTileColor(LevelTriggerBoxes[i].GetComponent<MapCubeManager>().sideA, WooHooFly.Colors.TileColor.green);
        }
        /*int index = 1;
        foreach (GameObject star in LevelStars)
        {
            string s = "LevelStars" + index.ToString();
            PlayerPrefs.SetInt(s, star.GetComponent<StarManager>().starNum);
        }*/

        MapTransition.instance.RedefineNode();
    }
}
