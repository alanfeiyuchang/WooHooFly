using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBottomCollider : MonoBehaviour
{
    private MapTag mapTag;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MapCube") {
            Debug.Log(other.transform.parent.gameObject.name + " info is obtained");
            mapTag = other.gameObject.GetComponent<MapTag>();

            CubeControllerTemp cubeController = CubeControllerTemp.instance;
            cubeController.edgeDirection = mapTag.EdgeDirection;
            cubeController.IsEdgeCube = mapTag.IsEdgeCube;

            if (mapTag.IsConnectCube)
            {
                cubeController.connectDirection = mapTag.ConnectDirection;
                cubeController.IsConnectCube = mapTag.IsConnectCube;
                cubeController.DownWall = mapTag.DownWall;
                cubeController.OnTheWall = mapTag.OnTheWall;

            }
                
        }
        
    }

private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MapCube") {
            CubeControllerTemp cubeController = CubeControllerTemp.instance;
            cubeController.IsConnectCube = false;
            cubeController.edgeDirection = new bool[4];
            cubeController.connectDirection = new bool[4];
            cubeController.DownWall = false;
            cubeController.OnTheWall = false;
        }
    }
}
