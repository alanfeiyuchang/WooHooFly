using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBaseRotation : MonoBehaviour
{
    bool isMoving = false;
    public float time = 3.0f;
    public float speed = 3.0f;
    private  bool leftRotate = false;
    public static MapBaseRotation instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {

        if (isMoving)
        {
            return;
        }

        if (leftRotate)
        {
            StartCoroutine(RotateMap(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(RotateMap(Vector3.back));
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
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;
        leftRotate = false;
    }
    public  void setRotate(bool rotate)
    {
        if(rotate == true)
        {
            leftRotate = true;
        }
    }
}
