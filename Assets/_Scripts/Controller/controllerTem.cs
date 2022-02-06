using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RW.MonumentValley;

public class controllerTem : MonoBehaviour
{
    [System.Serializable]
    public class Neighbor
    {
        public GameObject forwardNode = null;
        public GameObject leftNode = null;
        public GameObject backwardNode = null;
        public GameObject rightNode = null;

        public Node forward { get; set; }
        public Node left { get; set; }
        public Node right { get; set; }
        public Node backward { get; set; }

        public void Rest()
        {
            forward = null;
            left = null;
            right = null;
            backward = null;

            forwardNode = null;
            leftNode = null;
            backwardNode = null;
            rightNode = null;
        }
    }

    public float moveTime;
    public Neighbor avaiableNeighbor = new Neighbor();

    private Graph graph;
    private Node currentNode;

    private void Awake()
    {
        graph = FindObjectOfType<Graph>();
    }

    private void Start()
    {
        // always start on a Node
        SnapToNearestNode();
    }

    // snap the Player to the nearest Node in Game view
    public void SnapToNearestNode()
    {
        Node nearestNode = graph?.FindClosestNode(transform.position);
        if (nearestNode != null)
        {
            currentNode = nearestNode;
            transform.position = nearestNode.transform.position;
            transform.parent = nearestNode.transform;
            UpdateOrientation();
        }
    }

    public void UpdateOrientation()
    {
        Vector3 neighborPos;
        Vector3 currentPos;
        avaiableNeighbor.Rest();
        foreach (Edge edge in currentNode.GetComponent<Node>().Edges)
        {
            neighborPos = edge.neighbor.transform.position;
            currentPos = this.transform.position;
            float angle = Vector3.Angle(neighborPos - currentPos, Vector3.forward);
            float angle2 = Vector3.Angle(neighborPos - currentPos, Vector3.right);
            angle = (angle2 > 90) ? 360 - angle : angle;
            if (angle >= 260 && angle < 350)
            {
                avaiableNeighbor.forward = edge.neighbor;
                avaiableNeighbor.forwardNode = edge.neighbor.gameObject;
            }
            else if (angle >= 350 && angle < 360 || angle >=0 && angle < 80)
            {
                avaiableNeighbor.right = edge.neighbor;
                avaiableNeighbor.rightNode = edge.neighbor.gameObject;
            }
            else if (angle >= 80 && angle < 170)
            {
                avaiableNeighbor.backward = edge.neighbor;
                avaiableNeighbor.backwardNode = edge.neighbor.gameObject;
            }
            else if (angle >= 170 && angle < 260)
            {
                avaiableNeighbor.left = edge.neighbor;
                avaiableNeighbor.leftNode = edge.neighbor.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(MoveToNodeRoutine(avaiableNeighbor.forward));
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(MoveToNodeRoutine(avaiableNeighbor.left));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(MoveToNodeRoutine(avaiableNeighbor.backward));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(MoveToNodeRoutine(avaiableNeighbor.right));
        }
    }
    IEnumerator MoveToNodeRoutine(Node targetNode)
    {
        if(targetNode == null)
            yield break;

        float elapsedTime = 0;

        // validate move time
        //moveTime = Mathf.Clamp(moveTime, 0.1f, 5f);
        Vector3 startPos = this.transform.position;
        Vector3 targetPos = targetNode.transform.position;
        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpValue = Mathf.Clamp(elapsedTime / moveTime, 0f, 1f);

            transform.position = Vector3.Lerp(startPos, targetPos, lerpValue);

            if (lerpValue > 0.51f)
            {
                transform.parent = targetNode.transform;
                currentNode = targetNode;
                UpdateOrientation();
            }
            yield return null;
        }

    }
}
