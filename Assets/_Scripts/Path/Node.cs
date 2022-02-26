using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WooHooFly.Colors;

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
        [SerializeField] private Color transitCubeGizmoColor = Color.green;

        // neighboring nodes + active state
        [SerializeField] private List<Edge> edges = new List<Edge>();
        [SerializeField] private List<Edge> corners = new List<Edge>();
        [SerializeField] private List<TransitEdge> transits = new List<TransitEdge>();

        // Nodes specifically excluded from Edges
        [SerializeField] private List<Node> excludedNodes;

        // reference to the graph
        private Graph graph;

        // previous Node that forms a "breadcrumb" trail back to the start
        private Node previousNode;

        // the current Node belong to
        private GameObject Tile;

        // invoked when Player enters this node
        //public UnityEvent gameEvent;

        // properties

        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public List<Edge> Edges => edges;
        public List<Edge> Corners => corners;

        public List<TransitEdge> Transits => transits;

        private static Direction[] directionList = { Direction.Forward, Direction.Right, Direction.Backward, Direction.Left };

        private void Start()
        {
            Tile = this.transform.parent.gameObject.transform.GetChild(0).gameObject;
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
            // draws a line to node in same cube space
            foreach (Edge e in corners)
            {
                if (e.neighbor != null)
                {
                    Gizmos.color = sameCubeGizmoColor;
                    Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
                }
            }

            //draws a line to virtual node
            foreach(TransitEdge e in transits)
            {
                    if (e.neighbor != null)
                    {
                        Vector3 virtualNeigborPos = this.transform.position + GetTranslate(e.direction);
                        Gizmos.color = transitCubeGizmoColor;
                        Gizmos.DrawLine(transform.position, virtualNeigborPos);
                        Gizmos.DrawSphere(virtualNeigborPos, gizmoRadius);
                        Gizmos.DrawLine(virtualNeigborPos, e.neighbor.transform.position);
                    }
                
               
            }
        }


        // node blue axis is forward, red is right
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

        // based on the direction to get the translate Vector
        private Vector3 GetTranslate(Direction direction)
        {
            Vector3 translateVector = Vector3.zero;

            switch (direction)
            {
                case Direction.Forward:
                    translateVector = this.transform.forward;
                    break;
                case Direction.Left:
                    translateVector = -this.transform.right;
                    break;
                case Direction.Backward:
                    translateVector = -this.transform.forward;
                    break;
                case Direction.Right:
                    translateVector = this.transform.right;
                    break;
                case Direction.None:
                    break;
            }
            return translateVector;
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
                    Edge newEdge = new Edge { neighbor = newNode, isActive = true, direction = GetDirection(direction)};
                    edges.Add(newEdge);
                }
            }
        }

        // connect nodes in the same cube space
        public void FindCorners()
        {
            Vector3[] horizontalDirection = { transform.forward, -transform.forward, transform.right, -transform.right };
            Vector3[] verticalDirection = { transform.up, -transform.up };

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

        public void EnableTransitEdge(Node neighborNode, bool state)
        {
            foreach (TransitEdge e in transits)
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


        // Based on the graph, moving direction and current rotation of level, output startPos and EndPos

        public bool FindNodesAtDirection(ref Node startNode, ref Node endNode, ref Vector3 transitVector, ref bool translateBeforeRotate, Direction InputDirection, Direction worldDirect )
        {
            Direction direction = correctDirection(worldDirect, InputDirection);
            
            //find the transit node at that direction 
            foreach (TransitEdge e in transits)
            {
                
                if (e.neighbor.isActiveAndEnabled)
                {
                    if (e.direction == direction && e.isActive)
                    {
                        startNode = this;
                        endNode = e.neighbor;

                        translateBeforeRotate = !e.atFront;
                        Vector3 virtualNeigborPos = this.transform.position + GetTranslate(e.direction);
                        transitVector = endNode.transform.position - virtualNeigborPos;

                        return true;
                    }
                }
            }

            // find the neighbor node at that direction
            
            foreach (Edge e in edges)
            {
                if (e.direction == direction && e.isActive)
                {
                    
                    
                        startNode = this;
                        endNode = e.neighbor;
                        return true;
                    
                }
            }

            // check if other node at same cube space have neighbor node
            foreach (Edge c in corners)
            {
                Debug.Log(c.direction);
                Debug.Log(direction);
                if (c.direction == direction && c.isActive)
                {
                    foreach (Edge e in c.neighbor.Edges)
                    {
                        if (e.direction == direction && e.isActive)
                        {
                            startNode = c.neighbor;
                            endNode = e.neighbor;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Based on the rotationAngle and face correct direction
        // e.g. rotate level 90' to right, would make node on the top face: forward(edge)->right(input) ...
        private Direction correctDirection(Direction LevelDirect, Direction InputDirect)
        {
            Direction outputDirect = directionList[((int)InputDirect - (int)LevelDirect + 4) % 4];
            return outputDirect;
        }

        // public TileColor GetCurrentColor()
        // {
        //     // since only one tile is ative every time, we can use getComponent
        //     return side.GetComponentInChildren<TileManager>(false).MapColor;
        // }

        public GameObject GetTile()
        {
            return Tile;
        } 
    }


}