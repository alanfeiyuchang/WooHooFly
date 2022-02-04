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
<<<<<<< HEAD
            if (tag.IsConnectCube) {
                cubeController.connectDirection = tag.ConnectDirection;
                cubeController.IsConnectCube = tag.IsConnectCube;
=======
            cubeController.downwall = tag.downwall;
            //TODO - need add ground collider check! (ChuTing? Menghan?)
            // so only copy connect direction when the collision is on ground
            if (tag.IsConnectCube) { 
                cubeController.connectDirection = tag.ConnectDirection; 
>>>>>>> ad549720a89adb439c370c181d17a326f1b98d31
            }
            
        }
    }

    private void OnTriggerExit(Collider other) {
        CubeController cubeController = CubeController.instance;
        cubeController.connectDirection = new bool[4];
    }



    
}
