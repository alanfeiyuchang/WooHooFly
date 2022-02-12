using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCubeManager : MonoBehaviour
{
    [Header("Enable Node")]
    public bool NodeAEnabled;
    public bool NodeBEnabled;
    public bool NodeCEnabled;
    public bool NodeDEnabled;
    public bool NodeEEnabled;
    public bool NodeFEnabled;

    [Header("Enable Side")]
    public bool sideAEnabled;
    public bool sideBEnabled;
    public bool sideCEnabled;
    public bool sideDEnabled;
    public bool sideEEnabled;
    public bool sideFEnabled;
 
    public GameObject sideA;
    public GameObject sideB;
    public GameObject sideC;
    public GameObject sideD;
    public GameObject sideE;
    public GameObject sideF;
    


    void Start()
    {
        sideA = transform.GetChild(0).gameObject;
        sideB = transform.GetChild(1).gameObject;
        sideC = transform.GetChild(2).gameObject;
        sideD = transform.GetChild(3).gameObject;
        sideE = transform.GetChild(4).gameObject;
        sideF = transform.GetChild(5).gameObject;
    }


    void Update()
    {
        
    }
    
    public void ChangeSideActiveStatus(GameObject side, bool enable)
    {
        GameObject changableSide = side.transform.GetChild(0).gameObject;
        GameObject unchangableSide = side.transform.GetChild(1).gameObject;
        changableSide.SetActive(enable);
        unchangableSide.SetActive(!enable);
    }

    public void ChangeNodeActiveStatus(GameObject side, bool enable)
    {
        GameObject node = side.transform.GetChild(2).gameObject;
        node.SetActive(enable);
    }
}