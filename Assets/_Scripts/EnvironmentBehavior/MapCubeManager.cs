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

    [Header("Side Tile Type")]
    public TileType sideAType;
    public TileType sideBType;
    public TileType sideCType;
    public TileType sideDType;
    public TileType sideEType;
    public TileType sideFType;
 
    [HideInInspector]
    public GameObject sideA, sideB, sideC, sideD, sideE, sideF;
    
    public enum TileType
    {
        Unchangeable,
        Changeable,
        Color   
    };

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
    
    public void ChangeTileType(GameObject side, TileType type)
    {
        GameObject changableSide = side.transform.GetChild(0).gameObject;
        GameObject unchangableSide = side.transform.GetChild(1).gameObject;
        GameObject coloringSide = side.transform.GetChild(3).gameObject;
        changableSide.SetActive(false);
        unchangableSide.SetActive(false);
        coloringSide.SetActive(false);
        switch (type)
        {
            case TileType.Changeable:
                changableSide.SetActive(true);
                break;
            case TileType.Unchangeable:
                unchangableSide.SetActive(true);
                break;
            case TileType.Color:
                coloringSide.SetActive(true);
                break;
            default:
                break;
        }
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