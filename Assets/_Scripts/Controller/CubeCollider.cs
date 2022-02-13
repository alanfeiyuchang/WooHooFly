using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static CubeCollider instance;
    public enum color{
        green,
        red,
        grey
    };
    public color Color;
    public Material Green;
    public Material Red;
    public Material Grey;
    private MapColorChange map;

    private MapTag mapTag;
    
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
        
        //change mapCube's color
        if (other.gameObject.tag == "MapCube")
        {
            map = other.gameObject.GetComponent<MapColorChange>();
            map.ColorChange(Color);
   
        }
        //change playerCube's color
        else if(other.gameObject.tag == "ColorCube")
        {
            map = other.gameObject.GetComponent<MapColorChange>();
            color c = map.MapColor;
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
            switch (c)
            {
                case color.green:
                    mesh.material = Green;
                    break;
                case color.red:
                    mesh.material = Red;
                    break;
                case color.grey:
                    mesh.material = Grey;
                    break;
                default:
                    break;
            }
        }
    }




    
}
