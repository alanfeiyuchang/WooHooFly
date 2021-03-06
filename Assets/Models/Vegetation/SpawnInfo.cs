using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "SpawnInfo", menuName = "SpawnInfo", order = 1)]
public class SpawnInfo : ScriptableObject
{
    public GameObject[] Trees;
    public GameObject[] Flowers;
}

  // [System.Serializable] 
  // public struct Spawnee{ // structure for object information
  //          public GameObject obj; // Prefab
  //          public short chance; // (0 to 99) %
  //          public PlantType plantType;
  // }

  // public enum PlantType {
  //   Tree,
  //   Flower,
  //   Grass
  // }
