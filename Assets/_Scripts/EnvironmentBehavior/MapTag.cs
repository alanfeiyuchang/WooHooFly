using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTag : MonoBehaviour
{
    // Start is called before the first frame update

    public bool IsEdgeCube;
    public bool IsConnectCube;
    public bool OnTheWall;
    public bool DownWall;
    [Tooltip("0->foward,1->backward,2->left,3->right,(ex. if left is egde,EdgeDirection[2] = true")]
    public bool[] EdgeDirection = new bool[4];
    [Tooltip("0->foward,1->backward,2->left,3->right,(ex. if left is egde,ConnectDirection[2] = true")]
    public bool[] ConnectDirection = new bool[4];

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
