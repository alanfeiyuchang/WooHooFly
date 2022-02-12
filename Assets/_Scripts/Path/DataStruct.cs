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
        None = 4, Forward = 0, Right = 1, Backward = 2, Left = 3
    }
    [System.Serializable]
    public enum Face
    {
        None, Bottom, Top, Front, Back, Left, Right
    }
}
