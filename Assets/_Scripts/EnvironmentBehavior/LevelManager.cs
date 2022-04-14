using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> MapCubes;
    public List<GameObject> Flags;
    public GameObject PlayerCube;
    [HideInInspector]
    public GameObject Dummy;
    [Header("Finish requirements")]
    public List<GameObject> FinishCubes;
    public List<float> FinishAngles;


    private void Start()
    {
        MapTransition.instance.RedefineNode();
    }
}
