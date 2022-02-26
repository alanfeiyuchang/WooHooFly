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
    public enum Direction
    {
        None = 4, Forward = 0, Right = 1, Backward = 2, Left = 3
    }
    [System.Serializable]
    public enum Face
    {
        None, Bottom, Top, Front, Back, Left, Right
    }
    


}
