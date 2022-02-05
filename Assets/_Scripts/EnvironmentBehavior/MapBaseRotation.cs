using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseRotation : MonoBehaviour
{
    bool isMoving = false;
    public float time = 3.0f;
    public float speed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isMoving)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(RotateMap(Vector3.forward));
        }  
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(RotateMap(Vector3.back));
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartCoroutine(RotateMap(Vector3.right));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartCoroutine(RotateMap(Vector3.left));
        }
    }

    IEnumerator RotateMap(Vector3 direction)
    {
        float remainingAngle = 90;
        isMoving = true;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.Rotate(direction, rotationAngle);
            //transform.Rotate(direction, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }
        isMoving = false;
    }
}
