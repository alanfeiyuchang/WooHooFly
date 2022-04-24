using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;
using WooHooFly.Colors;

namespace TileSystem
{
    public class RiverGenerator : MonoBehaviour
    {
        public static RiverGenerator instance;
        private Graph graph;
        private List<Node> nodes;
        private Node northNode;
        private Node southNode;
        private Node eastNode;
        private Node westNode;
        private Transform environment;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            //create a gameobject contains real world item
            environment = this.transform.Find("Environment");
            if (environment == null)
            {
                environment = new GameObject("Environment").transform;
                environment.transform.parent = this.transform;
            }

            graph = Graph.instance;
            nodes = graph.GetAllNodes();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                GenerateRealWorld();
        }

        public void GenerateRealWorld()
        {

            //create a gameobject contains real world item
            environment = this.transform.Find("Environment");
            if (environment == null)
            {
                environment = new GameObject("Environment").transform;
                environment.transform.parent = this.transform;
            }

            nodes = graph.GetAllNodes();

            foreach (Node node in nodes)
            {
                
                if(!node.TileInfo.isWaterFall)
                    GenerateRiver(node, node.TileInfo.MapColor);
                else if(node.TileInfo.isWaterFall)
                    GenerateWaterFall(node, node.TileInfo.MapColor);
            }

        }

        private void GenerateWaterFall(Node node, TileColor color)
        {
            int TileID = 0;
            northNode = findColoredTileAtDirection(node, Direction.Forward, color);
            southNode = findColoredTileAtDirection(node, Direction.Backward, color);
            eastNode = findColoredTileAtDirection(node, Direction.Right, color);
            westNode = findColoredTileAtDirection(node, Direction.Left, color);
            

            if (northNode != null)
            {
                TileID += (int)BitmaskData.N;
            }
            if (eastNode != null)
            {
                TileID += (int)BitmaskData.E;
                TileID += (int)BitmaskData.SE;
                if (northNode != null)
                    TileID += (int)BitmaskData.NE;
            }
            if (westNode != null)
            {
                TileID += (int)BitmaskData.W;
                TileID += (int)BitmaskData.SW; 
                if (northNode != null)
                    TileID += (int)BitmaskData.NW;
            }
            TileID += (int)BitmaskData.S;
            TileColor playerColor = MapTransition.instance.GetCurrentLevel().PlayerCube.GetComponent<CubeCollider>().Color;
            if(playerColor == TileColor.green)
            {
                if(playerColor == color)
                {
                    GameObject Tile = Resources.Load<GameObject>("TileSet/WaterFallTiles/WaterFall_Tile" + TileID);
                    GameObject temp = Instantiate(Tile, node.transform.position, node.transform.rotation, environment);
                }
            }
            else if(playerColor == TileColor.red)
            {
                if(playerColor == color)
                {
                    GameObject Tile = Resources.Load<GameObject>("TileSet/LavaFallTiles/LavaFallTile_Tile" + TileID);
                    GameObject temp = Instantiate(Tile, node.transform.position, node.transform.rotation, environment);
                }
            }
            else
            {
                //SpawnEnviiorment.instance.spawnGround(node.transform);
            }


            //Vector3 waterToCenter =  node.transform.position - node.transform.parent.parent.position;
            //if (SpawnEnviiorment.instance != null)
            //    SpawnEnviiorment.instance.spawnTile(temp.transform, waterToCenter);
        }
        private void GenerateRiver(Node node, TileColor color)
        {
            int TileID = 0;
            northNode = findColoredTileAtDirection(node, Direction.Forward, color);
            southNode = findColoredTileAtDirection(node, Direction.Backward, color);
            eastNode = findColoredTileAtDirection(node, Direction.Right, color);
            westNode = findColoredTileAtDirection(node, Direction.Left, color);

            if (northNode != null)
                TileID += (int)BitmaskData.N;
            if (southNode != null)
                TileID += (int)BitmaskData.S;
            if (eastNode != null)
                TileID += (int)BitmaskData.E;
            if (westNode != null)
                TileID += (int)BitmaskData.W;


            if (findColoredTileAtDirection(northNode, Direction.Right, color) != null
                && findColoredTileAtDirection(eastNode, Direction.Forward, color) != null)
                TileID += (int)BitmaskData.NE;
            if (findColoredTileAtDirection(northNode, Direction.Left, color) != null
                && findColoredTileAtDirection(westNode, Direction.Forward, color) != null)
                TileID += (int)BitmaskData.NW;
            if (findColoredTileAtDirection(southNode, Direction.Right, color) != null
                && findColoredTileAtDirection(eastNode, Direction.Backward, color) != null)
                TileID += (int)BitmaskData.SE;
            if (findColoredTileAtDirection(southNode, Direction.Left, color) != null
                && findColoredTileAtDirection(westNode, Direction.Backward, color) != null)
                TileID += (int)BitmaskData.SW;
            GameObject Tile;
            TileColor playerColor = MapTransition.instance.GetCurrentLevel().PlayerCube.GetComponent<CubeCollider>().Color;
            if (playerColor == TileColor.green)
            {
                if (playerColor == color)
                {
                    Tile = Resources.Load<GameObject>("TileSet/WaterFallTiles/River_Tile" + TileID);
                    GameObject temp = Instantiate(Tile, node.transform.position, node.transform.rotation, environment);
                }
            }
            else if (playerColor == TileColor.red)
            {
                if (playerColor == color)
                {
                    Tile = Resources.Load<GameObject>("TileSet/LavaFallTiles/LavaTile_Tile" + TileID);
                    GameObject temp = Instantiate(Tile, node.transform.position, node.transform.rotation, environment);
                }
            }
            else
            {
                //white tiles become ground?
                //SpawnEnviiorment.instance.spawnGround(node.transform);
            }
            //if(SpawnEnviiorment.instance != null)
            //    SpawnEnviiorment.instance.spawnTile(temp.transform, waterToCenter);
        }

        private Node findColoredTileAtDirection(Node currentNode, Direction direction, TileColor color)
        {
            Node node;
            if (currentNode == null)
                return null;
            node = findColoredTileAtConnected(currentNode, direction, color);
            if (node != null)
                return node;

            node = findColoredTileAtCorner(currentNode, direction, color);
            if (node != null)
                return node;
            return null;
        }

        /// <summary>
        /// Find neighbor node which have same color
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="direction"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private Node findColoredTileAtConnected(Node currentNode, Direction direction, TileColor color)
        {
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

        private Node findColoredTileAtCorner(Node currentNode, Direction direction, TileColor color)
        {
            Node cornerNode;
            switch (direction)
            {
                case Direction.Forward:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.forward / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.forward / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Backward:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.forward / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.forward / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Left:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.right / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position - currentNode.transform.right / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                case Direction.Right:
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.right / 2 + currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    cornerNode = graph?.FindNodeAt(currentNode.transform.position + currentNode.transform.right / 2 - currentNode.transform.up / 2);
                    if (cornerNode != null && cornerNode.TileInfo.MapColor == color)
                        return cornerNode;
                    break;
                default:
                    break;
            }
            return null;
        }
    }
}

