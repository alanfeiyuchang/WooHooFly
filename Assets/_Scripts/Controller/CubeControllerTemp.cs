using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeControllerTemp : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 500;
    private bool moving = false;

    public Compass compassInfo;

    public bool[] edgeDirection = new bool[4];
    public bool[] connectDirection = new bool[4];
    public bool IsEdgeCube;
    public bool IsConnectCube;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
            return;
        if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(Roll(compassInfo.forwardPivot, compassInfo.forwardAxis));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            StartCoroutine(Roll(compassInfo.rightPivot, compassInfo.rightAxis));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Roll(compassInfo.backwardPivot, compassInfo.backwardAxis));
        }
        else if (Input.GetKey(KeyCode.A))
        {
            StartCoroutine(Roll(compassInfo.leftPivot, compassInfo.leftAxis));
        }
    }

    IEnumerator Roll(Vector3 rotationPointOffset, Vector3 rotationAxis)
    {
        float remainingAngle = 90;
        moving = true;
        Vector3 rotationPoint = this.transform.position + rotationPointOffset;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        moving = false;
    }
}
