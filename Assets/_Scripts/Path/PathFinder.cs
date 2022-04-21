using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;
using TileSystem;

namespace WooHooFly.NodeSystem
{
    // generates a path through a Graph
    [RequireComponent(typeof(Graph))]
    public class PathFinder : MonoBehaviour
    {

        // path start Node (usually current Node of the Player)
        [SerializeField] private Node startNode;

        // path end Node
        [SerializeField] private Node destinationNode;
        [SerializeField] private bool searchOnStart;

        // active angle
        private float activeAngle;

        // next Nodes to explore
        private List<Node> frontierNodes;

        // Nodes already explored
        private List<Node> exploredNodes;

        // Nodes that form a path to the goal Node (for Gizmo drawing)
        private List<Node> pathNodes;

        // is the search complete?
        private bool isSearchComplete;

        // has the destination been found?
        private bool isPathComplete;

        // structure containing all Nodes
        private Graph graph;
        private static Direction[] directionList = { Direction.Forward, Direction.Backward, Direction.Left, Direction.Right};

        // properties
        public Node StartNode { get { return startNode; } set { startNode = value; } }
        public Node DestinationNode { get { return destinationNode; } set { destinationNode = value; } }
        public List<Node> PathNodes => pathNodes;
        public bool IsPathComplete => isPathComplete;
        public bool SearchOnStart => searchOnStart;

        private void Awake()
        {
            graph = GetComponent<Graph>();
        }

        private void Start()
        {
            // if (searchOnStart)
            // {
            //     if (Input.GetKey("up")){
            //         pathNodes = FindPath();
            //     }

            // }
        }
        private void Update()
        {
            if (searchOnStart)
            {
                if (Input.GetKeyDown("up")){
                    pathNodes = FindPath();
                }
            }
        }

        private List<Node> SendPath(Node start, Node destination, float angle){
            pathNodes = FindPath(start, destination);
            if (angle == Mathf.RoundToInt(transform.eulerAngles.y)){
                return pathNodes;
            }
            else{
                return null;
            }
        }

        // initialize all Nodes/lists
        private void InitGraph()
        {
            // validate required components
            if (graph == null || startNode == null || destinationNode == null)
            {
                return;
            }

            frontierNodes = new List<Node>();
            exploredNodes = new List<Node>();
            pathNodes = new List<Node>();

            isSearchComplete = false;
            isPathComplete = false;

            // remove results of previous searches
            graph.ResetNodes();

            // first Node
            frontierNodes.Add(startNode);
        }

        // use a simple Breadth-first Search to explore one iteration
        private void ExpandFrontier(Node node)
        {
            Node cornerNode;
            Node impossibleNode;
            if (node == null)
            {
                return;
            }

            // loop through all Edges
            for (int i = 0; i < node.Edges.Count; i++)
            {   
                //Debug.Log(node.Edges.Count);
                // skip Edge if neighbor already explored or invalid
                if (node.Edges[i] == null ||
                    node.Edges.Count == 0 ||
                    exploredNodes.Contains(node.Edges[i].neighbor) ||
                    frontierNodes.Contains(node.Edges[i].neighbor))
                {
                    continue;
                }

                // create PreviousNode breadcrumb trail if Edge is active
                if (node.Edges[i].isActive && node.Edges[i].neighbor != null)
                {
  
                    if (node.Edges[i].neighbor.TileInfo.MapColor == node.TileInfo.MapColor)
                    {
                        // add neighbor Nodes to frontier Nodes
                        node.Edges[i].neighbor.PreviousNode = node;
                        frontierNodes.Add(node.Edges[i].neighbor);
                    }

                }
                foreach (Direction direction in directionList){
                    cornerNode = findColoredTileAtCorner(node, direction, node.TileInfo.MapColor);
                    if (cornerNode != null){
                        cornerNode.PreviousNode = node;
                        frontierNodes.Add(cornerNode);

                    }
                    
        
                }
                impossibleNode = findImpossilbleNode(node, node.TileInfo.MapColor);
                if (impossibleNode != null){
                    impossibleNode.PreviousNode = node;
                    frontierNodes.Add(impossibleNode);
                }
                
            }
        }

        private Node findImpossilbleNode(Node currentNode, TileColor color){
            Node impossibleNode;

            foreach (TransitEdge e in currentNode.Transits){
                Debug.Log(e);

                if (e.isActive){
                    impossibleNode = e.neighbor;
                    Debug.Log(impossibleNode);
                    if (impossibleNode.TileInfo.MapColor == color){
                        return impossibleNode;
                    }
                }
                Debug.Log(e.isActive);
            }
            return null;
        }
        private Node findColoredTileAtCorner(Node currentNode, Direction direction, TileColor color)
        {
            Node cornerNode;
            switch (direction)
            {
                case Direction.Forward:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.forward / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.forward / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Backward:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.forward / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.forward / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Left:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.right / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.right / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Right:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.right / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color){

                        Debug.Log(color);
                        Debug.Log(cornerNode.TileInfo.MapColor == color);
                        return cornerNode;
                    }

                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.right / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                default:
                    break;
            }
            return null;
        }

        // set the PathNodes from the startNode to destinationNode
        public List<Node> FindPath()
        {
            List<Node> newPath = new List<Node>();

            if (startNode == null || destinationNode == null || startNode == destinationNode)
            {
                return newPath;
            }

            // prevents infinite loop
            const int maxIterations = 100;
            int iterations = 0;

            // initialize all Nodes
            InitGraph();

            // search the graph until goal is found or all nodes explored (or exceeding some limit)
            while (!isSearchComplete && frontierNodes != null && iterations < maxIterations)
            {
                iterations++;

                // if we still have frontier Nodes to check
                if (frontierNodes.Count > 0)
                {
                    // remove the first Node
                    Node currentNode = frontierNodes[0];
                    frontierNodes.RemoveAt(0);

                    // and add to the exploredNodes
                    if (!exploredNodes.Contains(currentNode))
                    {
                        exploredNodes.Add(currentNode);
                    }

                    // add unexplored neighboring Nodes to frontier
                    ExpandFrontier(currentNode);

                    // if we have found the destination Node
                    if (frontierNodes.Contains(destinationNode))
                    {
                        // generate the Path to the goal
                        newPath = GetPathNodes();
                        isSearchComplete = true;
                        isPathComplete = true;
                        Debug.Log("Found dest: " + newPath.Count.ToString());
                    }
                }
                // if whole graph explored but no path found
                else
                {
                    isSearchComplete = true;
                    isPathComplete = false;
                }
            }
            return newPath;
        }

        public List<Node> FindPath(Node start, Node destination)
        {
            this.destinationNode = destination;
            this.startNode = start;
            return FindPath();
        }

        // find the best path given a bunch of possible Node destinations
        public List<Node> FindBestPath(Node start, Node[] possibleDestinations)
        {
            List<Node> bestPath = new List<Node>();
            foreach (Node n in possibleDestinations)
            {
                List<Node> possiblePath = FindPath(start, n);

                if (!isPathComplete && isSearchComplete)
                {
                    continue;
                }

                if (bestPath.Count == 0 && possiblePath.Count > 0)
                {
                    bestPath = possiblePath;
                }

                if (bestPath.Count > 0 && possiblePath.Count < bestPath.Count)
                {
                    bestPath = possiblePath;
                }
            }

            if (bestPath.Count <= 1)
            {
                ClearPath();
                return new List<Node>();
            }

            destinationNode = bestPath[bestPath.Count - 1];
            pathNodes = bestPath;
            return bestPath;
        }

        public void ClearPath()
        {
            startNode = null;
            destinationNode = null;
            pathNodes = new List<Node>();
        }

        // given a goal node, follow PreviousNode breadcrumbs to create a path
        public List<Node> GetPathNodes()
        {
            // create a new list of Nodes
            List<Node> path = new List<Node>();

            // start with the goal Node
            if (destinationNode == null)
            {
                return path;
            }
            path.Add(destinationNode);

            // follow the breadcrumb trail, creating a path until it ends
            Node currentNode = destinationNode.PreviousNode;

            while (currentNode != null)
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.PreviousNode;
            }
            return path;
        }

        private void OnDrawGizmos()
        {
            if (isSearchComplete)
            {
                foreach (Node node in pathNodes)
                {

                    if (node == startNode)
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawCube(node.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
                    }
                    else if (node == destinationNode)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawCube(node.transform.position, new Vector3(0.25f, 0.25f, 0.25f));
                    }
                    else
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(node.transform.position, 0.15f);
                    }

                    Gizmos.color = Color.yellow;
                    if (node.PreviousNode != null)
                    {
                        Gizmos.DrawLine(node.transform.position, node.PreviousNode.transform.position);
                    }
                }
            }
        }

        public void SetStartNode(Vector3 position)
        {
            StartNode = graph.FindClosestNode(position);
        }
    }
}