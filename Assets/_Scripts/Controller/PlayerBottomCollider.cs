using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBottomCollider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject playerCube;
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
        
    }
}
