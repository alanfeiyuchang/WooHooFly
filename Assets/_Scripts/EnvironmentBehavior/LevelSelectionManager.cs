using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelTriggerData
{
    public GameObject LevelTrigger;
    public int LevelIndex;
    public int Stars = 0;
}

public class LevelSelectionManager : MonoBehaviour
{
    public static LevelSelectionManager instance;
    private void Awake()
    {
        instance = this;
    }

    public List<LevelTriggerData> LevelTriggers;
    public Transform PlayerCubeTrans;


    private void Start()
    {
        LoadLevelSelection();
        UpdateLevelSelection();
    }

    public void LoadLevelSelection()
    {
        if (PlayerPrefs.HasKey("LevelUnlocked"))
            MapTransition.instance.LevelUnlocked = PlayerPrefs.GetInt("LevelUnlocked");
        foreach (LevelTriggerData data in LevelTriggers)
        {
            string s = "LevelStars" + (data.LevelIndex).ToString();
            if (PlayerPrefs.HasKey(s))
                data.Stars = PlayerPrefs.GetInt(s);
        }
    }

    public void UpdateLevelSelection()
    {
        PlayerPrefs.SetInt("LevelUnlocked", MapTransition.instance.LevelUnlocked);
        for (int i = 0; i < MapTransition.instance.LevelUnlocked; i++)
        {
            if (i >= LevelTriggers.Count)
                break;
            LevelTriggers[i].LevelTrigger.GetComponent<MapCubeManager>().sideAColor = WooHooFly.Colors.TileColor.green;
        }
        foreach (LevelTriggerData data in LevelTriggers)
        {
            string s = "LevelStars" + (data.LevelIndex).ToString();
            PlayerPrefs.SetInt(s, data.Stars);
        }
        MapTransition.instance.RedefineNode();
    }
}
