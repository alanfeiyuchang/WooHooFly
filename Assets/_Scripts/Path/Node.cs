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
        public Clickable ClickableTile { get; private set; }

        // invoked when Player enters this node
        //public UnityEvent gameEvent;

        // properties

        public Node PreviousNode { get { return previousNode; } set { previousNode = value; } }
        public List<Edge> Edges => edges;
        public List<Edge> Corners => corners;

        public List<TransitEdge> Transits => transits;

        private static Direction[] directionList = { Direction.Forward, Direction.Right, Direction.Backward, Direction.Left };

        // Analytics variables, Used to track heatmap data
        private int VisitedCount;
        private Vector3 InitalPosition;
        private Quaternion InitialRotation;

        private void Start()
        {
            Tile = this.transform.parent.gameObject.transform.GetChild(0).gameObject;
            ClickableTile = this.transform.parent.GetComponentInChildren<Clickable>();
            // automatic connect Edges with horizontal Nodes
            //if (graph != null)
            //{
            //    FindCorners();
            //    FindNeighbors();
            //}

            // Inital visited count for each node
            VisitedCount = 0;
            InitalPosition = this.transform.position;
            InitialRotation = this.transform.rotation;
            //Debug.Log("[Analytics]Position: " + InitalPosition + " VisitedCount=" + VisitedCount);
        }

        // increment visit count
        public void VisitNode()
        {
            this.VisitedCount++;
            Debug.Log("[Analytics]Position: " + InitalPosition + " Rotation: " + InitialRotation  + " VisitedCount=" + VisitedCount);
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
            Vector3[] cornerDirection = {   new Vector3(0.5f, 0.5f, 0f), new Vector3(0.5f, -0.5f, 0f),
                                            new Vector3(-0.5f, 0.5f, 0f), new Vector3(-0.5f, -0.5f, 0f),
                                            new Vector3(0.5f, 0f, 0.5f), new Vector3(0.5f, 0, -0.5f),
                                            new Vector3(-0.5f, 0f, 0.5f), new Vector3(-0.5f, 0, -0.5f),
                                            new Vector3(0f, 0.5f, 0.5f), new Vector3(0f, 0.5f, -0.5f),
                                            new Vector3(0f, -0.5f, 0.5f), new Vector3(0f, -0.5f, -0.5f),
            };

            // search through possible corners offsets, they are in the same cube space
            // 12 points
            //UR->R, UF->F, UL->L, UB->B, 
            //RF->R, RB->R, LF->L, LB->L
            //DR->R, DF->F, DL->L, DB->B

            foreach (Vector3 direction in cornerDirection)
            {
                Node newNode = graph?.FindNodeAt(transform.position + direction);

                if (newNode == null)
                    continue;

                if (this.transform.parent.parent == newNode.transform.parent.parent)
                {
                    // two node at same cube which is not traversable
                    continue;
                }

                Vector3 movingDirection = Vector3.zero;
                if (direction.x > 0)
                    movingDirection = transform.right;
                else if (direction.x < 0)
                    movingDirection = -transform.right;
                else if (direction.z > 0)
                    movingDirection = transform.forward;
                else if (direction.z < 0)
                    movingDirection = -transform.forward;

                // add to edges list if not already included and not excluded specifically
                if (!HasNeighbor(newNode) && !excludedNodes.Contains(newNode))
                {

                    Edge newEdge = new Edge { neighbor = newNode, isActive = true, direction = GetDirection(movingDirection) };
                    corners.Add(newEdge);
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

        public void EnableCornerEdge(Node cornerNode, bool state)
        {
            foreach (Edge e in corners)
            {
                if (e.neighbor.Equals(cornerNode))
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

        public NodeMovingInfo FindNodesAtDirection( Direction InputDirection, Direction worldDirect )
        {
            NodeMovingInfo movingInfo = new NodeMovingInfo();
            Direction direction = correctDirection(worldDirect, InputDirection);
            
            //find the transit node at that direction 
            foreach (TransitEdge e in transits)
            {
                
                if (e.neighbor.isActiveAndEnabled)
                {
                    if (e.direction == direction && e.isActive)
                    {
                        movingInfo.startNode = this;
                        movingInfo.endNode = e.neighbor;
                        movingInfo.transitVector = movingInfo.endNode.transform.position - this.transform.position - GetTranslate(e.direction);
                        if (e.atFront)
                        {
                            movingInfo.transitState = TransitState.RotateMove;
                            movingInfo.transitNodePos = this.transform.position + GetTranslate(e.direction);
                        }
                        else
                        {
                            movingInfo.transitState = TransitState.MoveRotate;
                            movingInfo.transitNodePos = e.neighbor.transform.position - GetTranslate(e.direction);
                        }

                        return movingInfo;
                    }
                }
            }

            // find the neighbor node at that direction
            
            foreach (Edge e in edges)
            {
                if (e.direction == direction && e.isActive)
                {
                    movingInfo.startNode = this;
                    movingInfo.endNode = e.neighbor;
                    movingInfo.transitState = TransitState.None;

                    return movingInfo;
                }
            }

            // check if other node at same cube space have neighbor node
            // if so it will found avaiable node path at neighbor node
            foreach (Edge c in corners)
            {
                if ( c.isActive)
                {
                    foreach (Edge e in c.neighbor.Edges)
                    {
                        //Direction neigborDirection = correctDirection(worldDirect, direction);
                        if (e.direction == direction && e.isActive)
                        {
                            movingInfo.startNode = c.neighbor;
                            movingInfo.endNode = e.neighbor;
                            movingInfo.transitState = TransitState.MoveRotate;
                            Vector3 startNodePos = this.transform.position + this.transform.up / 2;
                            Vector3 cornerNodePos = c.neighbor.transform.position + c.neighbor.transform.up / 2;
                            movingInfo.transitNodePos = c.neighbor.transform.position;
                            movingInfo.transitVector = cornerNodePos - startNodePos;
                            return movingInfo;
                        }
                    }
                }
            }
            return movingInfo;
        }


        public NodeMovingInfo MovingInfo(Node endNode)
        {
            NodeMovingInfo movingInfo = new NodeMovingInfo();

            //find the transit node at that direction 
            foreach (TransitEdge e in transits)
            {
                if(e.neighbor.isActiveAndEnabled && e.isActive && e.neighbor == endNode)
                {
                    movingInfo.startNode = this;
                    movingInfo.endNode = e.neighbor;
                    movingInfo.transitVector = endNode.transform.position - this.transform.position - GetTranslate(e.direction);
                    if (e.atFront)
                    {
                        movingInfo.transitState = TransitState.RotateMove;
                        movingInfo.transitNodePos = this.transform.position + GetTranslate(e.direction);
                    }
                    else
                    {
                        movingInfo.transitState = TransitState.MoveRotate;
                        movingInfo.transitNodePos = e.neighbor.transform.position - GetTranslate(e.direction);
                    }
                    
                    return movingInfo;
                }
            }

            // find the neighbor node at that direction
            foreach (Edge e in edges)
            {
                if (e.neighbor.isActiveAndEnabled && e.isActive && e.neighbor == endNode)
                {
                    movingInfo.startNode = this;
                    movingInfo.endNode = e.neighbor;
                    movingInfo.transitState = TransitState.None;
                    movingInfo.transitNodePos = e.neighbor.transform.position;
                    return movingInfo;
                }
            }
            // find the node at same cube space but different start node
            foreach (Edge c in corners)
            {
                if (c.isActive)
                {
                    foreach(Edge e in c.neighbor.Edges)
                    {
                        if(e.neighbor.isActiveAndEnabled && e.isActive && e.neighbor == endNode)
                        {
                            movingInfo.startNode = c.neighbor;
                            movingInfo.endNode = e.neighbor;
                            movingInfo.transitState = TransitState.MoveRotate;
                            Vector3 startNodePos = this.transform.position + this.transform.up / 2;
                            Vector3 cornerNodePos = c.neighbor.transform.position + c.neighbor.transform.up / 2;
                            movingInfo.transitNodePos = c.neighbor.transform.position;
                            movingInfo.transitVector = cornerNodePos - startNodePos;
                            return movingInfo;
                        }
                    }
                }
            }

            return movingInfo;
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