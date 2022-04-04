using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;

namespace WooHooFly.NodeSystem
{
    [System.Serializable]
    public class Edge
    {
        public Node neighbor;
        public bool isActive;
        public Direction direction;
    }

    [System.Serializable]
    public class TransitEdge
    {
        public Node neighbor;
        public bool isActive;
        public Direction direction;
        public bool atFront;
    }

    public class NodeMovingInfo
    {
        public Node startNode;
        public Node endNode;
        public Vector3 transitNodePos;
        public Vector3 transitVector;
        public TransitState transitState;
    }

    [System.Serializable]
    public class RotationLink
    {
        public float activeAngle;
        public GameObject checkNodeATransform;
        public GameObject checkNodeBTransform;
        public Vector3 transform;
        [Header("Nodes to activate")]
        public Node nodeA;
        public Node nodeB;
    }

    [System.Serializable]
    public class RotationLinker
    {
        public float activeAngle;
        [Header("Nodes to activate")]
        public Node nodeA;
        public Node nodeB;
        public string NodeA_Annotation;
        public string NodeB_Annotation;
    }

    [System.Serializable]
    public enum Direction
    {
        None = 4, Forward = 0, Right = 1, Backward = 2, Left = 3
    }
    [System.Serializable]
    public enum Face
    {
        None, Bottom, Top, Front, Back, Left, Right
    }
    public enum TransitState
    {
        MoveRotate, RotateMove, None
    }

    [System.Serializable]
    public class Path
    {
        public List<Node> path = new List<Node>();
        public void Clear() { path.Clear(); }
        public void Add(Node node) { path.Add(node); }
        public bool Contains(Node node) { return path.Contains(node); }

    }

}
