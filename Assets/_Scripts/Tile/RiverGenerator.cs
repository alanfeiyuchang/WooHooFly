using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;

namespace TileSystem
{
    public class RiverGenerator : MonoBehaviour
    {
        public Graph graph;
        private List<Node> nodes;
        private Node northNode;
        private Node southNode;
        private Node eastNode;
        private Node westNode;
        private Transform environment;

        private void Start()
        {
            //create a gameobject contains real world item
            environment = this.transform.Find("Environment");
            if (environment == null)
            {
                environment = new GameObject("Environment").transform;
                environment.transform.parent = this.transform;
            }
            
            nodes = graph.GetAllNodes();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                GenerateRealWorld();
        }

        private void GenerateRealWorld()
        {
            foreach (Node node in nodes)
            {
                if(node.TileInfo.MapColor == TileColor.green)
                    GenerateRiver(node);
            }
        }
        private void GenerateRiver(Node node)
        {
            int TileID = 0;
            northNode = findColoredTileAtDirection(node, Direction.Forward, TileColor.green);
            southNode = findColoredTileAtDirection(node, Direction.Backward, TileColor.green);
            eastNode = findColoredTileAtDirection(node, Direction.Right, TileColor.green);
            westNode = findColoredTileAtDirection(node, Direction.Left, TileColor.green);

            if (northNode != null)
                TileID += (int)BitmaskData.N;
            if (southNode != null)
                TileID += (int)BitmaskData.S;
            if (eastNode != null)
                TileID += (int)BitmaskData.E;
            if (westNode != null)
                TileID += (int)BitmaskData.W;


            if (findColoredTileAtDirection(northNode, Direction.Right, TileColor.green) != null
                && findColoredTileAtDirection(eastNode, Direction.Forward, TileColor.green) != null)
                TileID += (int)BitmaskData.NE;
            if (findColoredTileAtDirection(northNode, Direction.Left, TileColor.green) != null
                && findColoredTileAtDirection(westNode, Direction.Forward, TileColor.green) != null)
                TileID += (int)BitmaskData.NW;
            if (findColoredTileAtDirection(southNode, Direction.Right, TileColor.green) != null
                && findColoredTileAtDirection(eastNode, Direction.Backward, TileColor.green) != null)
                TileID += (int)BitmaskData.SE;
            if (findColoredTileAtDirection(southNode, Direction.Left, TileColor.green) != null
                && findColoredTileAtDirection(westNode, Direction.Backward, TileColor.green) != null)
                TileID += (int)BitmaskData.SW;

            GameObject Tile = Resources.Load<GameObject>("TileSet/Tile" + TileID);
            GameObject temp = Instantiate(Tile, node.transform.position, node.transform.rotation, environment);
            SpawnEnviiorment.instanace.spawnTile(temp);
        }

        /// <summary>
        /// Find neighbor node which have same color
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="direction"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public Node findColoredTileAtDirection(Node currentNode, Direction direction, TileColor color)
        {
            if (currentNode == null)
                return null;
            foreach (TransitEdge e in currentNode.Transits)
            {
                if (e.isActive && e.direction == direction)
                {
                    if(e.neighbor.TileInfo.MapColor == color)
                        return e.neighbor;
                }
            }
            foreach (Edge e in currentNode.Edges)
            {
                if (e.isActive && e.direction == direction)
                {
                    if (e.neighbor.TileInfo.MapColor == color)
                        return e.neighbor;
                }
            }
            return null;
        }
    }
}

