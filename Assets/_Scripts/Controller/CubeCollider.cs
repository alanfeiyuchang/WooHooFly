using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;

public class CubeCollider : MonoBehaviour
{
    // Start is called before the first frame update
    public static CubeCollider instance;
    //Will become this color once game starts
    public TileColor Color;
    private TileManager map;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        TileManager.instance.changeColor(gameObject, Color);
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
            TileColor c = map.MapColor;
            Debug.Log(c);
            Color = c;
            TileManager.instance.changeColor(gameObject, c);
        }
<<<<<<< HEAD
/*<<<<<<< HEAD
        else if(other.gameObject.tag == "Arrow")
        {
            ArrowManager.instance.NextArrow();
        }

=======
>>>>>>> parent of 568eaf6 (arrow sequence in tutorial one)*/
=======

>>>>>>> 812d58cb887d01f2287ecaa5c0b6d967f55a5724
    }




    
}
