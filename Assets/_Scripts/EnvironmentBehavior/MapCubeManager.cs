using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;

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

    [Header("Side Tile Color")]
    public TileColor sideAColor;
    public TileColor sideBColor;
    public TileColor sideCColor;
    public TileColor sideDColor;
    public TileColor sideEColor;
    public TileColor sideFColor;

    public Material Green;
    public Material Red;
    public Material Grey;
    public Material Blank;
    [HideInInspector]
    public GameObject sideA, sideB, sideC, sideD, sideE, sideF;
    
    public enum TileType
    {
        Unchangeable,
        Changeable,
        Color,
        Lock
    }
    private void OnValidate()
    {
        changeTileColor(sideA, sideAColor);
        changeTileColor(sideB, sideBColor);
        changeTileColor(sideC, sideCColor);
        changeTileColor(sideD, sideDColor);
        changeTileColor(sideE, sideEColor);
        changeTileColor(sideF, sideFColor);
    }
    public void changeAllTileColor(TileColor _color)
    {
        changeTileColor(sideA, _color);
        changeTileColor(sideB, _color);
        changeTileColor(sideC, _color);
        changeTileColor(sideD, _color);
        changeTileColor(sideE, _color);
        changeTileColor(sideF, _color);
    }
    public void changeAllTileType(TileType tileType)
    {
        ChangeTileType(sideA, tileType);
        ChangeTileType(sideB, tileType);
        ChangeTileType(sideC, tileType);
        ChangeTileType(sideD, tileType);
        ChangeTileType(sideE, tileType);
        ChangeTileType(sideF, tileType);
    }
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
    public void changeColor(GameObject ob, TileColor c)
    {
        MeshRenderer mesh = ob.GetComponent<MeshRenderer>();
        switch (c)
        {
            case TileColor.green:
                mesh.material = Green;
                break;
            case TileColor.red:
                mesh.material = Red;
                break;
            case TileColor.grey:
                mesh.material = Grey;
                break;
            case TileColor.none:
                mesh.material = Blank;
                break;
            default:
                break;
        }
    }
    public void changeTileColor(GameObject side, TileColor c)
    {
        GameObject tile = side.transform.GetChild(0).gameObject;
        //Debug.Log("Here" + tile.name);
        changeColor(tile, c);
        //Debug.Log("Here");
        TileManager tileManager = tile.GetComponent<TileManager>();
        tileManager.MapColor = c;
    }
    public void ChangeTileType(GameObject side, TileType type)
    {
        /*GameObject changableSide = side.transform.GetChild(0).gameObject;
        GameObject unchangableSide = side.transform.GetChild(1).gameObject;
        GameObject coloringSide = side.transform.GetChild(3).gameObject;
        GameObject defaultSide = side.transform.GetChild(4).gameObject;
        changableSide.SetActive(false);
        unchangableSide.SetActive(false);
        coloringSide.SetActive(false);
        defaultSide.SetActive(false);
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
            case TileType.DefalutTile:
                defaultSide.SetActive(true);
                break;
            default:
                break;
        }*/
        GameObject tile = side.transform.GetChild(0).gameObject;
        GameObject unchangeable = tile.transform.GetChild(0).gameObject;
        GameObject color = tile.transform.GetChild(1).gameObject;
        tile.SetActive(true);
        switch (type)
        {
            case TileType.Changeable:
                tile.tag = "ChangeTile";
                unchangeable.SetActive(false);
                color.SetActive(false);
                break;
            case TileType.Unchangeable:
                tile.tag = "UnchangeTile";
                TileManager tileManager = tile.GetComponent<TileManager>();
                if (tileManager.MapColor == TileColor.grey)
                {
                    unchangeable.SetActive(false);
                    color.SetActive(false);
                }
                else
                {
                    unchangeable.SetActive(true);
                    color.SetActive(false);
                }
                
                break;
            case TileType.Color:
                tile.tag = "ColorTile";
                unchangeable.SetActive(false);
                color.SetActive(true);
                break;
            case TileType.Lock:
                tile.tag = "UnchangeTile";
                unchangeable.SetActive(true);
                color.SetActive(false);
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
        GameObject node = side.transform.GetChild(1).gameObject;
        node.SetActive(enable);
    }
}