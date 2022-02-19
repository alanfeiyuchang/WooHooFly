using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;
using UnityEngine.Events;

public class CubeController : MonoBehaviour
{
    private Graph graph;
    private Node currentNode;
    private CubeCollider cubeCollider;
    private bool isMoving;
    Vector3 translateVector = Vector3.zero;
    bool translateBeforeRotate = false;
    Node startRollNode = null, endRollNode = null;


    //public IllusionSpot IllusionSpot;
    //public IllusionSpot IllusionSpot2;
    public GameObject SnapPoint;
    public float speed = 500;
    //public string JumpDirection;
    public UnityEvent RotationEvent;
    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        cubeCollider = this.GetComponent<CubeCollider>();
    }
    private void Start()
    {
        currentNode = graph?.FindClosestNode(SnapPoint.transform.position);
    }

    private void Update()
    {
        //if (IllusionSpot != null && IllusionSpot.ReadyForJump)
        //{
        //     JumpDirection = IllusionSpot.direction;
        //}
        //else if (IllusionSpot != null && IllusionSpot2.ReadyForJump)
        //{
        //    JumpDirection = IllusionSpot2.direction;
        //}
        //else
        //{
        //    JumpDirection = null;
        //}
        if (isMoving)
            return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            Rolling(Direction.Forward);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            //if(JumpDirection == "Left")
            //{
            //    IllusionSpot.IllusionJump();
            //    currenPos = SnapPoint.transform.position;
            //    SnapToNearestNode();
            //}
            Rolling(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Rolling(Direction.Backward);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //if (JumpDirection == "Right")
            //{
            //    IllusionSpot2.IllusionJump();
            //    currenPos = SnapPoint.transform.position;
            //    SnapToNearestNode();
            //}
            Rolling(Direction.Right);
        }
    }

    private void Rolling(Direction direction)
    {
        if (currentNode != null)
        {
            translateVector = Vector3.zero;
            TileColor currentColor = gameObject.GetComponent<CubeCollider>().Color;

            if (currentNode.FindNodesAtDirection(ref startRollNode, ref endRollNode,ref translateVector, ref translateBeforeRotate,  direction, GameManager.instance.levelDirection, currentColor))
            {
                //if (!CorrectColor(endRollNode))
                //    return;

                Vector3 currenPos = startRollNode.transform.position;
                Vector3 targetPos = endRollNode.transform.position;

                if (translateBeforeRotate)
                {
                    // translate then rotate
                    this.transform.position = this.transform.position + translateVector;
                    currenPos = currenPos + translateVector;
                }
                else
                {
                    // rotate then translate or there is no translate
                    targetPos = targetPos - translateVector;
                }

                Vector3 midPos = (currenPos + targetPos) / 2;
                Vector3 toTargetVector = targetPos - currenPos;
                Vector3 toCenterVector = this.transform.position - currenPos;

                StartCoroutine(Roll(midPos, Vector3.Cross(toCenterVector, toTargetVector)));

                RotationEvent.Invoke();
                //UIController.instance.AddStep();
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
        if (translateVector != Vector3.zero && !translateBeforeRotate)
        {
            // translate after finish rotation
            this.transform.position = this.transform.position + translateVector;
        }
        SnapToNearestNode();


    }

    private void SnapToNearestNode()
    {
        currentNode = endRollNode;
        roundPosition();
        SnapPoint.transform.position = endRollNode.transform.position;
    }

    private void roundPosition()
    {
        this.transform.localPosition =  new Vector3(Mathf.Round(this.transform.localPosition.x * 2) / 2, Mathf.Round(this.transform.localPosition.y * 2) / 2, Mathf.Round(this.transform.localPosition.z * 2) / 2);
        this.transform.localEulerAngles = new Vector3(Mathf.Round(transform.localEulerAngles.x / 90) * 90, Mathf.Round(transform.localEulerAngles.y / 90) * 90, Mathf.Round(transform.localEulerAngles.z / 90) * 90);
    }

    private bool CorrectColor(Node nextNode)
    {
        if(nextNode.GetCurrentColor() == cubeCollider.Color)
            return true;
        return false;
    }
}
