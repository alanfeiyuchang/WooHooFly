using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBottomCollider : MonoBehaviour
{
    private MapTag mapTag;
    [SerializeField] private GameObject playerCube;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 newVec = new Vector3(playerCube.transform.position.x,
        // playerCube.transform.position.y - 0.5f,playerCube.transform.position.z);
        // newVec = Quaternion.Euler(0, -45, 0) * newVec;
        // transform.position = newVec;
    }

    private void OnTriggerEnter(Collider other)
    {
         CubeControllerTemp cubeController = CubeControllerTemp.instance;   
        // if (cubeController.IsConnectCube) {
        //     if (cubeController.connectDirection[0] == true) {
        //         Debug.Log(0);
        //         transform.position = new Vector3(playerCube.transform.position.x,
        //         playerCube.transform.position.y,playerCube.transform.position.z - 0.5f);
        //         transform.Rotate(Vector3.up, 90);
        //     } else if(cubeController.connectDirection[1] == true)
        //     {
        //         Debug.Log(1);
        //         transform.position = new Vector3(playerCube.transform.position.x,
        //         playerCube.transform.position.y,playerCube.transform.position.z - 0.5f);
        //         transform.Rotate(Vector3.right, 90);
        //     } else if(cubeController.connectDirection[2] == true){
        //         Debug.Log(2);
        //         transform.position = new Vector3(playerCube.transform.position.x - 0.5f,
        //         playerCube.transform.position.y,playerCube.transform.position.z);
        //         transform.Rotate(Vector3.up, 90);
        //     } else if(cubeController.connectDirection[3] == true){
        //         Debug.Log(3);
        //         transform.position = new Vector3(playerCube.transform.position.x + 0.5f,
        //         playerCube.transform.position.y,playerCube.transform.position.z);
        //         transform.Rotate(Vector3.right, 4);
        //     }
        // }
 
        if (other.gameObject.tag == "MapCube") {

        mapTag = other.gameObject.GetComponent<MapTag>();


            cubeController.edgeDirection = mapTag.EdgeDirection;
            

            if (mapTag.IsConnectCube)
            {
                cubeController.connectDirection = mapTag.ConnectDirection;
                cubeController.IsConnectCube = mapTag.IsConnectCube;
            }
        }

        
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "MapCube") {
            CubeControllerTemp cubeController = CubeControllerTemp.instance;


            mapTag = other.gameObject.GetComponent<MapTag>();
            cubeController.connectDirection = mapTag.ConnectDirection;

            if (mapTag.IsConnectCube) {
                if (cubeController.connectDirection[0] == true) {
                    Debug.Log(0);
                    // transform.position = new Vector3(playerCube.transform.position.x,
                    // playerCube.transform.position.y,playerCube.transform.position.z + 0.5f);
                    // transform.Rotate(Vector3.left, 90);
                } else if(cubeController.connectDirection[1] == true)
                {
                    Debug.Log(1);
                    // transform.position = new Vector3(playerCube.transform.position.x,
                    // playerCube.transform.position.y,playerCube.transform.position.z - 0.5f);
                    // transform.Rotate(Vector3.right, 90);
                } else if(cubeController.connectDirection[2] == true){
                    Debug.Log(2);
                    // transform.position = new Vector3(playerCube.transform.position.x - 0.5f,
                    // playerCube.transform.position.y,playerCube.transform.position.z);
                    // transform.Rotate(Vector3.forward, -90);
                } else if(cubeController.connectDirection[3] == true){
                    Debug.Log(3);
                    transform.position = new Vector3(playerCube.transform.position.x + 0.5f,
                    playerCube.transform.position.y,playerCube.transform.position.z);
                    transform.Rotate(Vector3.forward, 90);
                }
            }



            
            cubeController.IsConnectCube = false;
            cubeController.edgeDirection = new bool[4];
            cubeController.connectDirection = new bool[4];
        }
    }
}
