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

    public int StartCount(int step)
    {
        if (step >= OneStarStep)
            return 0;
        else if (step >= TwoStarStep)
            return 1;
        else if (step >= ThreeStarStep)
            return 2;
        else
            return 3;
    }
}
[CreateAssetMenu(fileName= "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    //public int totalLevelNum;
    public List<levelStar> levelStarData;
}
