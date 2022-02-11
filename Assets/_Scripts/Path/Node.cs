using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WooHooFly.NodeSystem
{
    public class Node : MonoBehaviour
    {
        // gizmo colors
        [SerializeField] private float gizmoRadius = 0.1f;
        [SerializeField] private Color defaultGizmoColor = Color.black;
        [SerializeField] private Color selectedGizmoColor = Color.blue;
        [SerializeField] private Color inactiveGizmoColor = Color.gray;
        [SerializeField] private Color sameCubeGizmoColor = Color.yellow;

        // neighboring nodes + active state
        [SerializeField] private List<Edge> edges = new List<Edge>();
        [SerializeField] private List<Edge> corners = new List<Edge>();

        // Nodes specifically excluded from Edges
        [SerializeField] private List<Node> excludedNodes;

        // reference to the graph
        private Graph graph;

        // previous Node that forms a "breadcrumb" trail back to the start
        private Node previousNode;

        // invoked when Player enters this node
        //public UnityEvent gameEvent;

        // properties

        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public List<Edge> Edges => edges;
        public List<Edge> Corners => corners;

        private void Start()
        {
            // automatic connect Edges with horizontal Nodes
            //if (graph != null)
            //{
            //    FindCorners();
            //    FindNeighbors();
            //}
        }

        // draws a sphere gizmo
        private void OnDrawGizmos()
        {
            Gizmos.color = defaultGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);
        }

        // draws a sphere gizmo in a different color when selected
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = selectedGizmoColor;
            Gizmos.DrawSphere(transform.position, gizmoRadius);

            // draws a line to each neighbor
            foreach (Edge e in edges)
            {
                if (e.neighbor != null)
                {
                    Gizmos.color = (e.isActive) ? selectedGizmoColor : inactiveGizmoColor;
                    Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
                }
            }
            // draws 
            foreach (Edge e in corners)
            {
                if (e.neighbor != null)
                {
                    Gizmos.color = sameCubeGizmoColor;
                    Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
                }
            }
        }

        // fill out edge connections to neighboring nodes automatically
        public void FindNeighbors()
        {
            Vector3[] neighborDirections = { transform.forward, -transform.forward, transform.right, -transform.right };
             
            // search through possible neighbor offsets
            foreach (Vector3 direction in neighborDirections)
            {
                Node newNode = graph?.FindNodeAt(transform.position + direction);

                // add to edges list if not already included and not excluded specifically
                if (newNode != null && !HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
                {
                    Edge newEdge = new Edge { neighbor = newNode, isActive = true, direction = GetDirection(direction) };
                    edges.Add(newEdge);
                }
            }
        }

        // connect nodes in the same cube space
        public void FindCorners()
        {
            Vector3[] horizontalDirection = { transform.forward, -transform.forward, transform.right, -transform.right };
            Vector3[] verticalDirection = { transform.up, -transform.up};

            // search through possible corners offsets, they are in the same cube space
            foreach (Vector3 horizontaldirection in horizontalDirection)
            {
                foreach (Vector3 verticaldirection in verticalDirection)
                {
                    Vector3 direction = horizontaldirection / 2 + verticaldirection / 2;

                    Node newNode = graph?.FindNodeAt(transform.position + direction);

                    if (newNode == null)
                        continue;
                    
                    if (this.transform.parent.parent == newNode.transform.parent.parent)
                    {
                        // two node at same cube which is not traversable
                        continue;
                    }

                    // add to edges list if not already included and not excluded specifically
                    if (!HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
                    {
                        Edge newEdge = new Edge { neighbor = newNode, isActive = true, direction = GetDirection(horizontaldirection) };
                        corners.Add(newEdge);
                    }
                }
               
            }
        }

        // is a Node already in the Edges List?
        private bool HasNeighbor(Node node)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor != null && e.neighbor.Equals(node))
                {
                    return true;
                }
            }
            return false;
        }

        // given a specific neighbor, sets active state
        public void EnableEdge(Node neighborNode, bool state)
        {
            foreach (Edge e in edges)
            {
                if (e.neighbor.Equals(neighborNode))
                {
                    e.isActive = state;
                }
            }
        }

        public void InitGraph(Graph graphToInit)
        {
            this.graph = graphToInit;
        }

        private Direction GetDirection(Vector3 horizontalDir)
        {
            Direction direction;

            switch (horizontalDir)
            {
                case Vector3 dir when dir.Equals(transform.forward):
                    direction = Direction.Forward;
                    break;
                case Vector3 dir when dir.Equals(-transform.forward):
                    direction = Direction.Backward;
                    break;
                case Vector3 dir when dir.Equals(transform.right):
                    direction = Direction.Right;
                    break;
                case Vector3 dir when dir.Equals(-transform.right):
                    direction = Direction.Left;
                    break;
                default:
                    direction = Direction.None;
                    break;
            }

            return direction;
        }

        public bool FindNodesAtDirection(ref Vector3 startPos, ref Vector3 endPos, Direction direction)
        {
            // find the neighbor node at that direction
            foreach (Edge e in edges)
            {
                if(e.direction == direction && e.isActive)
                {
                    startPos = this.transform.position;
                    endPos = e.neighbor.transform.position;
                    return true;
                }
            }

            // check if other node at same cube space have neighbor node
            foreach(Edge c in corners)
            {
                if(c.direction == direction && c.isActive)
                {
                    foreach(Edge e in c.neighbor.Edges)
                    {
                        if(e.direction == direction && e.isActive)
                        {
                            startPos = c.neighbor.transform.position;
                            endPos = e.neighbor.transform.position;
                            return true;
                        }
                    }
                }
            }
            return false ;
        }
    }
}