using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
public class CubeControllerNew : MonoBehaviour
{
    private Graph graph;
    private Node currentNode;
    private Vector3 currenPos;
    private Vector3 targetPos;
    private bool isMoving;

    public GameObject SnapPoint;
    public float speed = 500;
    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
    }
    private void Start()
    {
        currenPos = SnapPoint.transform.position;
        SnapToNearestNode();
    }

    private void Update()
    {
        if (isMoving)
            return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            Rolling(Direction.Forward);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Rolling(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Rolling(Direction.Backward);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Rolling(Direction.Right);
        }
    }

    private void Rolling(Direction direction)
    {
        if (currentNode != null)
        {
            if (currentNode.FindNodesAtDirection(ref currenPos, ref targetPos, direction, GameManager.instance.levelDirection))
            {
                Vector3 midPos = (currenPos + targetPos) / 2;

                Vector3 toTargetVector = targetPos - currenPos;
                Vector3 toCenterVector = this.transform.position - currenPos;

                StartCoroutine(Roll(midPos, Vector3.Cross(toCenterVector, toTargetVector)));
            }
        }
    }

    IEnumerator Roll(Vector3 rotationPoint, Vector3 rotationAxis)
    {
        float remainingAngle = 90;
        isMoving = true;
        GameManager.instance.CurrentState = GameManager.GameState.rotating;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;

            yield return null;
        }

        GameManager.instance.CurrentState = GameManager.GameState.playing;
        isMoving = false;
        currenPos = targetPos;
        SnapToNearestNode();
    }

    private void SnapToNearestNode()
    {
        Node nearestNode = graph?.FindClosestNode(currenPos);
        if (nearestNode != null)
        {
            currentNode = nearestNode;
            roundPosition();
            SnapPoint.transform.position = currenPos;
        }
    }

    private void roundPosition()
    {
        this.transform.localPosition =  new Vector3(Mathf.Round(this.transform.localPosition.x * 2) / 2, Mathf.Round(this.transform.localPosition.y * 2) / 2, Mathf.Round(this.transform.localPosition.z * 2) / 2);
        this.transform.localEulerAngles = new Vector3(Mathf.Round(transform.localEulerAngles.x / 90) * 90, Mathf.Round(transform.localEulerAngles.y / 90) * 90, Mathf.Round(transform.localEulerAngles.z / 90) * 90);
    }
}
