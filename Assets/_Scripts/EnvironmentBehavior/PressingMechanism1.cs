using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingMechanism1 : MonoBehaviour
{
    // Start is called before the first frame update
    public bool AppearMechanism;
    public bool MoveMechanism;
    private bool checkmove = false;
    public Vector3 end;
    public float speed;
    public GameObject MapPart;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (checkmove)
        {
            DoMoveMechanism();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.name == "PlayerCube_OneColor")
        {
            if (AppearMechanism)
            {
                DoAppearMechanism();
            }
            else if (MoveMechanism)
            {
                checkmove = true;
            }
        }
    }
    private void DoAppearMechanism()
    {
        MapPart.SetActive(true);
    }
    private void DoMoveMechanism()
    {
        MapPart.transform.localPosition = Vector3.MoveTowards(MapPart.transform.localPosition, end, speed * Time.deltaTime);
    }
}
