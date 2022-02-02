using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class findroute : MonoBehaviour
{
    public Transform TargetObject;
    // Start is called before the first frame update
    void Start()
    {
        if (TargetObject != null)
            GetComponent<NavMeshAgent>().destination = TargetObject.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


