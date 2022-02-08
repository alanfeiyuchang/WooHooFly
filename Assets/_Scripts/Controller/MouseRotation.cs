using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseRotation : MonoBehaviour
{
    public Transform Compass;
    public float angle;
    private Vector3 rotationAngle;
    private Vector3 targetAngle;

    public UnityEvent rotationEvent;
    void Update()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        rotationAngle = Compass.transform.up * angle;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            targetAngle += Input.GetAxis("Mouse ScrollWheel") * rotationAngle * 10;
            transform.eulerAngles = clampAngle(targetAngle);
            //StartCoroutine(RotateMap(Compass.transform.up, angle));

            if (rotationEvent != null)
                rotationEvent.Invoke();
        }
    }

    //IEnumerator RotateMap(Vector3 direction, float angle)
    //{
    //    float t = 0;
    //    float duration = 3;
    //    while (t < duration)
    //    {
    //        t ++;
    //        float rotationAngle = Mathf.Lerp(0, angle, t / duration);
    //        transform.Rotate(direction, rotationAngle);

    //        GameManager.instance.CurrentState = GameManager.GameState.rotating;
    //        yield return null;
    //    }
    //    GameManager.instance.CurrentState = GameManager.GameState.playing;
    //}

    private Vector3 clampAngle(Vector3 target)
    {
        float x;
        if (target.x > 180)
            x = target.x - 360;
        else if (target.x < -180)
            x = target.x + 360;
        else
            x = target.x;

        float y;
        if (target.y > 180)
            y = target.y - 360;
        else if (target.y < -180)
            y = target.y + 360;
        else
            y = target.y;

        float z;
        if (target.z > 180)
            z = target.z - 360;
        else if (target.z < -180)
            z = target.z + 360;
        else
            z = target.z;
        return new Vector3(x, y, z);
    }
}
