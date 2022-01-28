using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int speed = 300;//��ת���ٶȣ�����ֱ����unity�޸�
    bool isMoving = false;

    void Update()
    {
        if (isMoving)//����Ҫ��һ����ת��ɣ����ܽ�����һ����ת
        {
            return;
        }
        //�������õ�����������
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
