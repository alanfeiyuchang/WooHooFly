using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static CubeCollider instance;
    public TileManager.colors Color;
    private TileManager map;

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
        if (other.gameObject.tag == "ChangeTile")
        {
            map = other.gameObject.GetComponent<TileManager>();
            map.ColorChange(Color);
   
        }
        //change playerCube's color
        else if(other.gameObject.tag == "ColorTile")
        {
            
            map = other.gameObject.GetComponent<TileManager>();
            TileManager.colors c = map.MapColor;
            Debug.Log(c);
            Color = c;
            TileManager.instance.changeColor(gameObject, c);
        }
    }




    
}
