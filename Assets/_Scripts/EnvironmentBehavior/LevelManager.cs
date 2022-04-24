using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WooHooFly.NodeSystem;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<GameObject> MapCubes;
    public List<GameObject> Flags;
    public GameObject PlayerCube;
    public float CameraProjectionSize = 6f;
    [HideInInspector]
    public GameObject Dummy;
    /*[Header("Finish requirements")]
    public List<GameObject> FinishCubes;
    public List<float> FinishAngles;*/
    public Node StartNode;
    public Node FinishNode;

    public bool NeedAutomaticRotation = false;
    public float FinalRotationAngle;

    private void Start()
    {
        MapTransition.instance.RedefineNode();
    }
}
