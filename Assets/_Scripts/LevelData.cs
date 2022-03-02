using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class levelStar
{
    public int OneStarStep;
    public int TwoStarStep;
    public int ThreeStarStep;
    public int StarEarned;
}
[CreateAssetMenu(fileName= "LevelData", menuName = "LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    //public int totalLevelNum;
    public List<levelStar> levelStarData;
}
