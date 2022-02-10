using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playerCube;
    

    private MapTag mapTag;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerCube.transform.position.x,
        playerCube.transform.position.y,playerCube.transform.position.z);
    }


}
