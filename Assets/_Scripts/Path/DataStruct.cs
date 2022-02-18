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

        public GameObject Tile;

        public Material getColor() {
            if (Tile.GetComponent<MeshRenderer>().enabled) {
                return Tile.GetComponent<MeshRenderer>().sharedMaterial;
            }
            return null;
        }
        
        public bool isWalkable(Material playerColor) {
            Material m1 = getColor();
            Material m2 = playerColor;
            //Debug.Log("Playcube is " + m2.name + "; Mapcube is " + (m1 == null ? "invisible" : m1.name) + " Tile is " + Tile.tag);
            if (m1 == m2 || Tile.tag == "ColorTile") {
                return true;
            }
            return false;
        }

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
