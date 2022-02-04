using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int speed = 300;//��ת���ٶȣ�����ֱ����unity�޸�
    bool isMoving = false;

    public bool[] edgeDirection = new bool[4];

    public bool[] connectDirection = new bool[4];

    public static CubeController instance;

    void Update()
    {
        
    }

    void Awake() {
        instance = this;
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
            if (edgeDirection[3] == true) {
                return;
            }
            
            StartCoroutine(Roll(Vector3.right));
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (edgeDirection[2] == true) {
                return;
            }
            StartCoroutine(Roll(Vector3.left));
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (edgeDirection[0] == true) {
                return;
            }
            StartCoroutine(Roll(Vector3.forward));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (edgeDirection[1] == true) {
                return;
            }
            StartCoroutine(Roll(Vector3.back));
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (connectDirection[0])
                StartCoroutine(Roll(Vector3.up)); 
        }
        // when is cube going south?
        else if (Input.GetKey(KeyCode.S))
        {
            if (connectDirection[1])
                StartCoroutine(Roll(Vector3.down));
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
