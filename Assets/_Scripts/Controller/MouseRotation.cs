using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public Vector3 rotationAngle;
    public Vector3 targetAngle;
    void Update()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetAngle += Input.GetAxis("Mouse ScrollWheel") * rotationAngle * 10f;
            transform.eulerAngles = targetAngle;
        }
    }
}
