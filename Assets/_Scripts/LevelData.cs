using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class levelStar
{
    public int LevelIndex;
    public int OneStarStep;
    public int TwoStarStep;
    public int ThreeStarStep;
    public int StarEarned;
    public void StarCount(int step)
    {
        if (step > OneStarStep && StarEarned <= 0)
            StarEarned = 1;
        else if (step > TwoStarStep && StarEarned <= 1)
            StarEarned = 1;
        else if (step > ThreeStarStep && StarEarned <= 2)
            StarEarned = 2;
        else
            StarEarned = 3;
        string s = "LevelStars" + LevelIndex.ToString();
        PlayerPrefs.SetInt(s, StarEarned);
        Debug.Log(StarEarned);
    }
}
[CreateAssetMenu(fileName= "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    //public int totalLevelNum;
    public List<levelStar> levelStarData;
}
