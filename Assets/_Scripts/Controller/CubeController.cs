using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public int speed = 300;
    bool isMoving = false;

    public bool[] edgeDirection = new bool[4];

    public bool[] connectDirection = new bool[4];
    public bool IsEdgeCube;
    public bool IsConnectCube;
    public MapBaseRotation mapBaseRotation;
    //public GameObject mapmapBaseRotation;

    public static CubeController instance;

    void Update()
    {
        if (isMoving)
        {
            return;
        }

        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            
            if (IsConnectCube && connectDirection[3])
            {
                StartCoroutine(Roll(Vector3.up));
            }
            else if(edgeDirection[3] == false)
            {
                return;
            }
            else
            { 
                StartCoroutine(Roll(Vector3.right));
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            
            if (IsConnectCube && connectDirection[2])
            {
                StartCoroutine(Roll(Vector3.up));
            }
            else if (edgeDirection[2] == false)
            {
                return;
            }
            else
            {
                StartCoroutine(Roll(Vector3.left));
            }
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            
            if (IsConnectCube && connectDirection[0])
            {
                StartCoroutine(Roll(Vector3.up));
            }
            else if (edgeDirection[0] == false)
            {
                return;
            }
            else
            {
                StartCoroutine(Roll(Vector3.forward));
            }
                
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            
            if (IsConnectCube && connectDirection[1])
            {
                StartCoroutine(Roll(Vector3.up));
            }
            else if (edgeDirection[1] == false)
            {
                return;
            }
            else
            {
                StartCoroutine(Roll(Vector3.back));
            }
              
        }
        else if (Input.GetKey(KeyCode.W))
        {
            if (connectDirection[0] || connectDirection[2] || connectDirection[3])
                StartCoroutine(Roll(Vector3.up));
        }
        // when is cube going south? 
        else if (Input.GetKey(KeyCode.S))
        {
            if (IsConnectCube)
                StartCoroutine(Roll(Vector3.down));
        }
        
    }

    void Awake() {
        instance = this;
    }

    void FixedUpdate()
    {
    
        
    }


    IEnumerator Roll(Vector3 direction)
    {
        isMoving = true;
        Vector3 rotationPoint;
        Vector3 rotationAxis ;
        float remainingAngle = 90;
        if (direction == Vector3.up && connectDirection[0])
        {
            rotationPoint = transform.position + Vector3.forward / 2 + Vector3.up / 2;
            rotationAxis = new Vector3(1,0,0);
        }
        else if (direction == Vector3.up && connectDirection[1])
        {
            rotationPoint = transform.position + Vector3.back / 2 + Vector3.up / 2;
            rotationAxis = new Vector3(-1, 0, 0);
        }
        else if (direction == Vector3.up && connectDirection[2])
        {
            
            rotationPoint = transform.position + Vector3.left / 2 + Vector3.up / 2;
            rotationAxis = new Vector3(0, 0, 1);
        }
        else if (direction == Vector3.up && connectDirection[3])
        {
            rotationPoint = transform.position + Vector3.right / 2 + Vector3.up / 2;
            rotationAxis = new Vector3(0, 0, -1);
        }
        else if (direction == Vector3.down && connectDirection[0])
        {
            rotationPoint = transform.position + Vector3.forward / 2 + Vector3.down / 2;
            rotationAxis = new Vector3(-1, 0, 0);
        }
        else if (direction == Vector3.down && connectDirection[1])
        {
            rotationPoint = transform.position + Vector3.back / 2 + Vector3.down / 2;
            rotationAxis = new Vector3(1, 0, 0);
        }
        else if (direction == Vector3.down && connectDirection[2])
        {
            rotationPoint = transform.position + Vector3.left / 2 + Vector3.down / 2;
            rotationAxis = new Vector3(0, 0, -1);
        }
        else if (direction == Vector3.down && connectDirection[3])
        {
            rotationPoint = transform.position + Vector3.right / 2 + Vector3.down / 2;
            rotationAxis = new Vector3(0, 0, 1);
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
        UIController.instance.AddStep();
        /*
        if(direction == Vector3.up && rotationAxis == new Vector3(0, 0, 1))
        {
            mapBaseRotation.setRotate("left");
        }
        else if (direction == Vector3.up && rotationAxis == new Vector3(0, 0, -1))
        {
            mapBaseRotation.setRotate("right");
        }*/
        Debug.Log("direction" + connectDirection[2]);
        isMoving = false;
    }
}
