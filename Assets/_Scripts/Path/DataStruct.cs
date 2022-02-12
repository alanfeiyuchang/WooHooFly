using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public enum Direction
    {
        None, Forward, Left, Backward, Right
    }
}
