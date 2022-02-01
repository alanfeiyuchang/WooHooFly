using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int speed = 300;//��ת���ٶȣ�����ֱ����unity�޸�
    bool isMoving = false;
    bool onWall = false;

    void Update()
    {
        
    }
    void FixedUpdate()
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
        else if (Input.GetKey(KeyCode.W))
        {
            StartCoroutine(Roll(Vector3.up));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Roll(Vector3.down));
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "wall")
        {
            Debug.Log("fs" + other.gameObject.tag);
            onWall = true;
        }
        else {
            Debug.Log("safe area");
            onWall = false;
        }
    }
    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;
        Vector3 rotationPoint;
        Vector3 rotationAxis ;
        float remainingAngle = 90;
        if (direction == Vector3.up)
        {
            rotationPoint = transform.position + Vector3.forward / 2 + Vector3.up / 2;
            rotationAxis = new Vector3(1,0,0);
        }
        else if (direction == Vector3.down)
        {
            rotationPoint = transform.position + Vector3.forward / 2 + Vector3.down / 2;
            rotationAxis = new Vector3(-1, 0, 0);
        }
        else
        {
            rotationPoint = transform.position + direction / 2 + Vector3.down / 2;
            rotationAxis = Vector3.Cross(Vector3.up, direction);

        }
        
        

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
