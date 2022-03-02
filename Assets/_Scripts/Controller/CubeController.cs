using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;
using UnityEngine.Events;

public enum InputType { MouseInput, KeyboardInput}

public class CubeController : MonoBehaviour
{
    private Graph graph;
    private Node currentNode;
    private CubeCollider cubeCollider;
    private bool isMoving;
    private Clickable[] clickables;
    Vector3 translateVector = Vector3.zero;
    bool translateBeforeRotate = false;
    Node startRollNode = null, endRollNode = null;

    public InputType inputType;
    public GameObject SnapPoint;
    public float speed = 500;
    public UnityEvent RotationEvent;
    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        cubeCollider = this.GetComponent<CubeCollider>();
        clickables = FindObjectsOfType<Clickable>();
    }
    private void Start()
    {
        foreach (Clickable c in clickables)
        {
            c.clickAction += OnClick;
        }
    }

    private void OnDisable()
    {
        // unsubscribe from clickEvents when disabled
        foreach (Clickable c in clickables)
        {
            c.clickAction -= OnClick;
        }
    }
    private void OnClick(Clickable clickable, Vector3 position)
    {

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
        if (GameManager.instance.CurrentState != GameManager.GameState.playing) 
            return;
        if (currentNode == null)
        {
            currentNode = graph?.FindClosestNode(SnapPoint.transform.position);
        }

        translateVector = Vector3.zero;
        TileColor currentColor = gameObject.GetComponent<CubeCollider>().Color;

        if (currentNode.FindNodesAtDirection(ref startRollNode, ref endRollNode, ref translateVector, ref translateBeforeRotate, direction, GameManager.instance.levelDirection, currentColor))
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

            //RotationEvent.Invoke();
            if(UIController.instance != null)
                UIController.instance.AddStep();
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
