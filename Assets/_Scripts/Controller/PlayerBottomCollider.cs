using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playerCube;
    

    private MapTag tag;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerCube.transform.position.x,
        playerCube.transform.position.y - 0.5f,playerCube.transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "MapCube") {
                
                tag = other.gameObject.GetComponent<MapTag>();

                CubeControllerTemp cubeController = CubeControllerTemp.instance;
                cubeController.edgeDirection = tag.EdgeDirection;
                

                if (tag.IsConnectCube)
                {
                    cubeController.connectDirection = tag.ConnectDirection;
                    cubeController.IsConnectCube = tag.IsConnectCube;

                    // cubeControllerTemp.connectDirection = tag.ConnectDirection;
                    // cubeControllerTemp.IsConnectCube = tag.IsConnectCube;
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
            }
        }

}
