using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;
    private void Awake()
    {
        instance = this;
    }
    public colors MapColor;
    public Material Green;
    public Material Red;
    public Material Grey;
    public enum colors
    {
        green,
        red,
        grey
    };
    // Start is called before the first frame update
    void Start()
    {
        GameObject cube = gameObject.transform.parent.parent.gameObject;
        GameObject parent = gameObject.transform.parent.gameObject;
        MapCubeManager cubeManager = cube.GetComponent<MapCubeManager>();
        switch (parent.name)
        {
            case "Side A":
                MapColor = cubeManager.sideAColor;
                break;
            case "Side B":
                MapColor = cubeManager.sideBColor;
                break;
            case "Side C":
                MapColor = cubeManager.sideCColor;
                break;
            case "Side D":
                MapColor = cubeManager.sideDColor;
                break;
            case "Side E":
                MapColor = cubeManager.sideEColor;
                break;
            case "Side F":
                MapColor = cubeManager.sideFColor;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void matchColor(colors c)
    {
        //Debug.Log(c);
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        switch (c)
        {
            case colors.green:
                mesh.material = Green;
                break;
            case colors.red:
                mesh.material = Red;
                break;
            case colors.grey:
                mesh.material = Grey;
                break;
            default:
                break;
        }
        GameObject cube = gameObject.transform.parent.parent.gameObject;
        GameObject parent = gameObject.transform.parent.gameObject;
        MapCubeManager cubeManager = cube.GetComponent<MapCubeManager>();
        switch (parent.name)
        {
            case "Side A":
                cubeManager.sideAColor = c;
                break;
            case "Side B":
                cubeManager.sideBColor = c;
                break;
            case "Side C":
                cubeManager.sideCColor = c;
                break;
            case "Side D":
                cubeManager.sideDColor = c;
                break;
            case "Side E":
                cubeManager.sideEColor = c;
                break;
            case "Side F":
                cubeManager.sideFColor = c;
                break;
        }
    }
    public void ColorChange(colors color)
    {
        MapColor = color;
        changeColor(gameObject, color);
        //GameManager.instance.CheckWin();
    }
    public void changeColor(GameObject ob, colors c)
    {
        MeshRenderer mesh = ob.GetComponent<MeshRenderer>();
        switch (c)
        {
            case colors.green:
                mesh.material = Green;
                break;
            case colors.red:
                mesh.material = Red;
                break;
            case colors.grey:
                mesh.material = Grey;
                break;
            default:
                break;
        }
    }
}
