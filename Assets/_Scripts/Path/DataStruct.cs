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

        public GameObject Tile;

        public TileColor getColor() {
            if (Tile.GetComponent<MeshRenderer>().enabled) {
                return Tile.GetComponent<TileManager>().MapColor;
            }
            return TileColor.none;
        }
        
        public bool isWalkable(TileColor playerColor) {
            TileColor m1 = getColor();
            TileColor m2 = playerColor;
            Debug.Log("Playcube is " + m2.ToString() + "; Mapcube is " + m1.ToString() + "; Tile is " + Tile.tag);
            if (m1.Equals(m2) || Tile.tag != "UnchangeTile") {
                return true;
            }
            return false;
        }

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
