using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static CubeCollider instance;
    public enum color{
        green,
        red
    };
    public color SideColor;
    
    private MapColorChange map;

    private MapTag tag;
    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
 
    }


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "MapCube")
        {
            Debug.Log(gameObject.name + " hit " + other.gameObject.name);
            map = other.gameObject.GetComponent<MapColorChange>();
            map.ColorChange(SideColor);
            tag = other.gameObject.GetComponent<MapTag>();
            CubeController cubeController = CubeController.instance;
            cubeController.edgeDirection = tag.EdgeDirection;
            if (tag.IsConnectCube) {
                cubeController.connectDirection = tag.ConnectDirection;
                cubeController.IsConnectCube = tag.IsConnectCube;
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        CubeController cubeController = CubeController.instance;
        cubeController.connectDirection = new bool[4];
    }



    
}
