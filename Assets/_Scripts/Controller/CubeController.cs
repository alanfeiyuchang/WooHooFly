using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;
using UnityEngine.Events;

public enum InputType { MouseInput, KeyboardInput }
[System.Serializable]
public class CubeController : MonoBehaviour
{
    private Graph graph;
    private Node currentNode;
    public CubeCollider cubeColorControl;
    private bool isMoving;
    private bool firstRotate;
    private bool firstClick = true;
    private Clickable[] clickables;
    private NodeMovingInfo movingInfo;
    private InputType inputType = InputType.MouseInput;

    public GameObject SnapPoint;
    public float speed = 500;
    public UnityEvent RotationEvent;
    public UnityEvent<Node, Node> rotationLayerEvent;
    protected AudioSource RollMusic;
    public Queue<Node> movingPath;
    [HideInInspector]
    public Node CurrentNode { get { return currentNode;}  set{ CurrentNode = currentNode; } }
    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
        cubeColorControl = this.GetComponent<CubeCollider>();
        clickables = FindObjectsOfType<Clickable>();
    }
    private void Start()
    {
        RollMusic = GetComponent<AudioSource>();
        foreach (Clickable c in clickables)
        {
            c.clickAction += OnClick;
        }

        currentNode = graph?.FindClosestNode(SnapPoint.transform.position);
        // [Analytics] increment current node visit count
        currentNode.VisitNode();
        SnapToNearestNode();

        FindAccessibleNode();
    }

    #region mouse_controller
    private void OnDisable()
    {
        // unsubscribe from clickEvents when disabled
        foreach (Clickable c in clickables)
        {
            c.clickAction -= OnClick;
        }
    }

    private void OnEnable()
    {
        // subscribe from clickEvents when enabled
        foreach (Clickable c in clickables)
        {
            c.clickAction += OnClick;
        }
    }
    private void OnClick(Clickable clickable, Vector3 position)
    {
        if (firstClick)
        {
            firstClick = false;
            FindAccessibleNode();
        }
        if (inputType != InputType.MouseInput)
            return;

        if (isMoving)
            return;

        movingPath = graph.GetPath(clickable.clickedNode);
        firstRotate = true;
        if(movingPath.Count!= 0)
            Rolling(movingPath.Dequeue());
    }

    public void FindAccessibleNode()
    {
        if (inputType == InputType.MouseInput)
            graph?.FindAccessibleNode(currentNode);
    }
    #endregion


    #region keyboard_control    
    private void Update()
    {
        if (inputType != InputType.KeyboardInput)
            return;
        if (isMoving)
            return;
        if (Input.GetKeyDown(KeyCode.W))
        {
            RollAtDirection(Direction.Forward);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            RollAtDirection(Direction.Left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            RollAtDirection(Direction.Backward);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            RollAtDirection(Direction.Right);
        }
    }

    private void RollAtDirection(Direction direction)
    {
        //movingInfo = currentNode.FindNodesAtDirection(direction, GameManager.instance.levelDirection);
        //Rolling();
    }
    #endregion


    private void Rolling(Node targetNode)
    {
        movingInfo = currentNode.MovingInfo(targetNode);
        if (GameManager.instance.CurrentState != GameManager.GameState.playing && GameManager.instance.CurrentState != GameManager.GameState.starting)
            return;

        if (movingInfo.endNode == null)
            return;

        if (!CorrectColor(movingInfo.endNode))
        {
            StopRolling();
            return;
        }

        Vector3 rotateStartPos = movingInfo.startNode.transform.position;
        Vector3 rotateEndPos = movingInfo.endNode.transform.position;

        if (movingInfo.transitState == TransitState.MoveRotate)
        {
            //this.transform.position = movingInfo.startNode.transform.position + movingInfo.startNode.transform.up;
            // translate then rotate
            this.transform.position = this.transform.position + movingInfo.transitVector;
            rotateStartPos = movingInfo.transitNodePos;
        }
        else if(movingInfo.transitState == TransitState.RotateMove)
        {
            // rotate then translate
            rotateEndPos = movingInfo.transitNodePos;
        }

        Vector3 midPos = (rotateStartPos + rotateEndPos) / 2;
        Vector3 toTargetVector = rotateEndPos - rotateStartPos;
        Vector3 toCenterVector = this.transform.position - rotateStartPos;

        StartCoroutine(Roll(midPos, Vector3.Cross(toCenterVector, toTargetVector)));

        //RotationEvent.Invoke();
        if(UIController.instance != null)
            UIController.instance.AddStep();
    }

    IEnumerator Roll(Vector3 rotationPoint, Vector3 rotationAxis)
    {
        RollMusic.Play();
        float remainingAngle = 90;
        isMoving = true;
        firstRotate = false;
        GameManager.instance.CurrentState = GameManager.GameState.rotating;
        rotationLayerEvent.Invoke(movingInfo.startNode, movingInfo.endNode);

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;

            yield return null;
        }

        if (movingInfo.transitState == TransitState.RotateMove)
        {
            // translate after finish rotation
            this.transform.position = this.transform.position + movingInfo.transitVector;
        }

        currentNode = movingInfo.endNode;
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        isMoving = false;

        // [Analytics] increment current node visit count
        currentNode.VisitNode();

        SnapToNearestNode();
        if(movingPath.Count != 0)
            Rolling(movingPath.Dequeue());
        else
            FindAccessibleNode();
    }

    private void StopRolling()
    {
        SnapToNearestNode();
        FindAccessibleNode();
    }

    private void SnapToNearestNode()
    {
        roundPosition();
        SnapPoint.transform.position = currentNode.transform.position;

        // change tile color 
        cubeColorControl.NewTileEnter(currentNode.TileInfo);
        foreach (Edge edge in currentNode.Corners)
        {
            if (edge.isActive)
            {
                cubeColorControl.NewTileEnter(edge.neighbor.TileInfo);
            }
        }

        // check win if current node is end node
        if (currentNode.TileInfo.isFinishTile)
        {
            GameManager.instance.CheckWin(currentNode.TileInfo.finalMap);
        }
    }

    private void roundPosition()
    {
        this.transform.localPosition =  new Vector3(Mathf.Round(this.transform.localPosition.x * 2) / 2, Mathf.Round(this.transform.localPosition.y * 2) / 2, Mathf.Round(this.transform.localPosition.z * 2) / 2);
        this.transform.localEulerAngles = new Vector3(Mathf.Round(transform.localEulerAngles.x / 90) * 90, Mathf.Round(transform.localEulerAngles.y / 90) * 90, Mathf.Round(transform.localEulerAngles.z / 90) * 90);
    }

    private bool CorrectColor(Node nextNode)
    {
        TileColor currentColor = cubeColorControl.Color;
        GameObject mapTile = nextNode.GetTile();
        TileColor mapColor = mapTile.GetComponent<TileManager>().MapColor;
        if (currentColor.Equals(mapColor) || mapTile.tag == "ChangeTile" || (mapTile.tag == "ColorTile" && firstRotate))
        {
            return true;
        }
        return false;
    }
}
