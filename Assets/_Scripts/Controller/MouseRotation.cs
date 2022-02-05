using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour
{
    public Vector3 rotationAngle;
    public Vector3 targetAngle;

    public UnityEvent rotationEvent;
    void Update()
    {
        
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetAngle += Input.GetAxis("Mouse ScrollWheel") * rotationAngle * 10f;
            transform.eulerAngles = targetAngle;


            if (rotationEvent != null)
                rotationEvent.Invoke();
        }

    }
}
