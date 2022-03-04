using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace WooHooFly.NodeSystem
{
    // management class for all Nodes
    public class Graph : MonoBehaviour
    {
        // all of the Nodes in the current level/maze
        private List<Node> allNodes = new List<Node>();

        // end of level
        private Node goalNode;
        private Node GoalNode => goalNode;

        [SerializeField] private List<Node> accessibleNodes = new List<Node>();

        public List<Node> AccessibleNodes => accessibleNodes;

        private void Awake()
        {
            allNodes = FindObjectsOfType<Node>().ToList();
            InitNodes();
        }

        private void Start()
        {
            InitNeighbors();
        }

        public void ReInitPath()
        {
            allNodes = FindObjectsOfType<Node>().ToList();
            InitNodes();
            InitNeighbors();
        }

        // locate the specific Node at target position within rounding error
        public Node FindNodeAt(Vector3 pos)
        {
            foreach (Node n in allNodes)
            {
                Vector3 diff = n.transform.position - pos;

                if (diff.sqrMagnitude < 0.01f)
                {
                    return n;
                }
            }
            return null;
        }

        // locate the closest Node in screen space, given an array of Nodes
        public Node FindClosestNode(Node[] nodes, Vector3 pos)
        {
            Node closestNode = null;
            float closestDistanceSqr = Mathf.Infinity;

            foreach (Node n in nodes)
            {
                Vector3 diff = n.transform.position - pos;

                Vector3 nodeScreenPosition = Camera.main.WorldToScreenPoint(n.transform.position);
                Vector3 screenPosition = Camera.main.WorldToScreenPoint(pos);
                diff = nodeScreenPosition - screenPosition;

                if (diff.sqrMagnitude < closestDistanceSqr)
                {
                    closestNode = n;
                    closestDistanceSqr = diff.sqrMagnitude;
                }
            }
            return closestNode;
        }

        // find the closest Node in the entire Graph
        public Node FindClosestNode(Vector3 pos)
        {
            return FindClosestNode(allNodes.ToArray(), pos);
        }

        // clear breadcrumb trail
        public void ResetNodes()
        {
            foreach (Node node in allNodes)
            {
                node.PreviousNode = null;
            }
        }

        // set the Graph for each Node
        private void InitNodes()
        {
            foreach (Node n in allNodes)
            {
                if (n != null)
                {
                    n.InitGraph(this);
                }
            }
        }

        // set neighbors for each Node; must run AFTER all Nodes are initialized
        private void InitNeighbors()
        {
            foreach (Node n in allNodes)
            {
                if (n != null)
                {
                    n.FindNeighbors();
                    n.FindCorners();
                }
            }
        }

        public void FindAccessibleNode(Node currentNode)
        {
            EnableClikable(false);
            accessibleNodes.Clear();
            foreach (TransitEdge e in currentNode.Transits)
            {
                if (e.neighbor.isActiveAndEnabled && e.isActive)
                {
                    accessibleNodes.Add(e.neighbor); 
                }
            }
            foreach (Edge e in currentNode.Edges)
            {
                if (e.isActive)
                {
                    accessibleNodes.Add(e.neighbor);
                }
            }
            foreach (Edge c in currentNode.Corners)
            {
                if (c.isActive)
                {
                    foreach (Edge e in c.neighbor.Edges)
                    {
                        if (e.isActive)
                        {
                            accessibleNodes.Add(e.neighbor);
                        }
                    }
                }
            }
            EnableClikable(true);
        }

        private void EnableClikable(bool enabled)
        {
            foreach(Node node in accessibleNodes)
            {
                node.ClickableTile.EnablePointer(enabled);
            }
        }

    }

}