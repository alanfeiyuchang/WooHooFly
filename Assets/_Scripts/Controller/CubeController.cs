using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int speed = 300;//翻转的速度，可以直接在unity修改
    bool isMoving = false;

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (isMoving)//必须要等一次旋转完成，才能进行下一次旋转
        {
            return;
        }
        //我现在用的是上下左右
        if (Input.GetKey(KeyCode.RightArrow))
        {
            StartCoroutine(Roll(Vector3.right));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            StartCoroutine(Roll(Vector3.back));
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("fs"+collision.gameObject.name);
    }
        IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;

        float remainingAngle = 90;
        Vector3 rotationPoint = transform.position + direction / 2 + Vector3.down / 2;
        Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;
            yield return null;
        }

        isMoving = false;
    }
}
