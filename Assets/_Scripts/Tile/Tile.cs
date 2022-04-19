using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileSystem
{
    [System.Serializable]
    public struct TileInfo
    {
        public bool subtile1;
        public bool subtile2;
        public bool subtile3;
        public bool subtile4;
        public bool subtile5;
        public bool subtile6;
        public bool subtile7;
        public bool subtile8;
        public bool subtile9;
    }

    public enum BitmaskData { NW = 1, N = 2, NE = 4, W = 8, E = 16, SW = 32, S = 64, SE = 128 }
    public enum TileType { RiverTile, WaterFallTile, LavaTile, LavaFallTile}
    public class Tile : MonoBehaviour
    {
        public GameObject grassTile;
        public GameObject waterTile;
        public TileInfo tileInfo;
        public float waterSizeRatio = 0.5f;
        [HideInInspector]
        public int tileBit;
        public TileType tileType;

        private List<Vector3> invisibleWaterTilePos = new List<Vector3>() {  new Vector3(-0.25f, 0, 0.5f), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0.5f),
                                                                                new Vector3(-0.25f, 0, 0), new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0),
                                                                                new Vector3(-0.25f, 0, -0.25f), new Vector3(0, 0, -0.25f), new Vector3(0.5f, 0, -0.25f),
        };

        //private float subtileSize = 0.25f;
        //private Vector3 subtilePos;
        //private Vector3 subtileScale;
        //private int subtilePos_i;
        //private int subtilePos_j;

        private bool isWaterTile(int index)
        {
            switch (index)
            {
                case 1: return tileInfo.subtile1;
                case 2: return tileInfo.subtile2;
                case 3: return tileInfo.subtile3;
                case 4: return tileInfo.subtile4;
                case 5: return tileInfo.subtile5;
                case 6: return tileInfo.subtile6;
                case 7: return tileInfo.subtile7;
                case 8: return tileInfo.subtile8;
                case 9: return tileInfo.subtile9;
            }
            return false;
        }

        public void GenerateTile()
        {
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    Vector3 position = new Vector3(this.transform.localPosition.x + ((float)(x - 1) * (waterSizeRatio + 1) / 4), this.transform.localPosition.y, this.transform.localPosition.z - ((float)(y - 1) * (waterSizeRatio + 1) / 4));
                    int index = x + y * 3 + 1;
                    GameObject newTile;
                    if (isWaterTile(index))
                        newTile = Instantiate(waterTile, position, Quaternion.identity, this.transform);
                    else
                        newTile = Instantiate(grassTile, position, Quaternion.identity, this.transform);

                    if (x == 1)
                        newTile.transform.localScale = new Vector3(newTile.transform.localScale.x * waterSizeRatio, newTile.transform.localScale.y, newTile.transform.localScale.z);
                    else
                        newTile.transform.localScale = new Vector3(newTile.transform.localScale.x * (1 - waterSizeRatio) / 2, newTile.transform.localScale.y, newTile.transform.localScale.z);

                    if (y == 1)
                        newTile.transform.localScale = new Vector3(newTile.transform.localScale.x, newTile.transform.localScale.y, newTile.transform.localScale.z * waterSizeRatio);
                    else
                        newTile.transform.localScale = new Vector3(newTile.transform.localScale.x, newTile.transform.localScale.y, newTile.transform.localScale.z * (1 - waterSizeRatio) / 2);

                    if (isWaterTile(index))
                    {
                        GameObject invisible_water_tile = new GameObject("Water_Tile");
                        invisible_water_tile.transform.position = invisibleWaterTilePos[index - 1];
                        invisible_water_tile.transform.parent = this.transform;
                        invisible_water_tile.transform.localScale = new Vector3(newTile.transform.localScale.x, 0.2f , newTile.transform.localScale.z);
                    }

                }
            }
            tileBit = getBitMaskValue();
        }
        //public void GenerateTile()
        //{
        //    subtilePos = new Vector3(this.transform.localPosition.x - (subtileSize * 1.5f), this.transform.localPosition.y, this.transform.localPosition.z + (subtileSize * 1.5f));
        //    subtileScale = new Vector3(waterTile.transform.localScale.x * subtileSize, waterTile.transform.localScale.y, waterTile.transform.localScale.z * subtileSize);


        //    subtilePos_i = 0;

        //    generateSubTile(isWaterTile(1), 0, 0);
        //    generateSubTile(isWaterTile(2), 1, 0);
        //    generateSubTile(isWaterTile(2), 2, 0);
        //    generateSubTile(isWaterTile(3), 3, 0);
        //    generateSubTile(isWaterTile(4), 0, 1);
        //    generateSubTile(isWaterTile(5), 1, 1);
        //    generateSubTile(isWaterTile(5), 2, 1);
        //    generateSubTile(isWaterTile(6), 3, 1);
        //    generateSubTile(isWaterTile(4), 0, 2);
        //    generateSubTile(isWaterTile(5), 1, 2);
        //    generateSubTile(isWaterTile(5), 2, 2);
        //    generateSubTile(isWaterTile(6), 3, 2);
        //    generateSubTile(isWaterTile(7), 0, 3);
        //    generateSubTile(isWaterTile(8), 1, 3);
        //    generateSubTile(isWaterTile(8), 2, 3);
        //    generateSubTile(isWaterTile(9), 3, 3);


        //    tileBit = getBitMaskValue();
        //} 
        //private void generateSubTile(bool isWaterTile, int i, int j)
        //{
        //    GameObject newTile;
        //    if (isWaterTile)
        //        newTile = Instantiate(waterTile, new Vector3(subtilePos.x + i * subtileSize, subtilePos.y, subtilePos.z - j * subtileSize), Quaternion.identity, this.transform);
        //    else
        //        newTile = Instantiate(grassTile, new Vector3(subtilePos.x + i * subtileSize, subtilePos.y, subtilePos.z - j * subtileSize), Quaternion.identity, this.transform);
        //    newTile.transform.localScale = subtileScale;
        //}

        private int getBitMaskValue()
        {
            int result = 0;
            result += (int)BitmaskData.NW * System.Convert.ToInt32(isWaterTile(1));
            result += (int)BitmaskData.N * System.Convert.ToInt32(isWaterTile(2));
            result += (int)BitmaskData.NE * System.Convert.ToInt32(isWaterTile(3));
            result += (int)BitmaskData.W * System.Convert.ToInt32(isWaterTile(4));
            result += (int)BitmaskData.E * System.Convert.ToInt32(isWaterTile(6));
            result += (int)BitmaskData.SW * System.Convert.ToInt32(isWaterTile(7));
            result += (int)BitmaskData.S * System.Convert.ToInt32(isWaterTile(8));
            result += (int)BitmaskData.SE * System.Convert.ToInt32(isWaterTile(9));

            result *= System.Convert.ToInt32(isWaterTile(5));

            return result;
        }
    }

}