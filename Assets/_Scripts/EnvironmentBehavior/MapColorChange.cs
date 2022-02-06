using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapColorChange : MonoBehaviour
{
    [SerializeField] private Material red;
    [SerializeField] private Material green;
    public CubeCollider.color MapColor;
    // Start is called before the first frame update
    void Start()
    {
        ColorChange(MapColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ColorChange(CubeCollider.color color)
    {
        Debug.Log(gameObject.name + " has became " + color);
        //change the color of the cube this collison belongs to
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        MapColor = color;
        switch (color)
        {
            case CubeCollider.color.green:
                mesh.material = green;
                break;
            case CubeCollider.color.red:
                mesh.material = red;
                break;
            default:
                break;
        }
    }
}
