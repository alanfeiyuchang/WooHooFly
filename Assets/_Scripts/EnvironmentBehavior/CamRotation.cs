using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    public float speed = 90f;
    private bool isRotating = false;
    private bool clockwise = true;

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            clockwise = true;
            StartCoroutine(RotateCam());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            clockwise = false;
            StartCoroutine(RotateCam());
        }
        //transform.Rotate(Vector3.up * speed * Time.deltaTime);
    }

    IEnumerator RotateCam()
    {
        float remainingAngle = 90;
        isRotating = true;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            if (clockwise)
            {
                transform.Rotate(Vector3.up, rotationAngle);
            }
            else
            {
                transform.Rotate(Vector3.up, -rotationAngle);
            }
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isRotating = false;
    }
}
