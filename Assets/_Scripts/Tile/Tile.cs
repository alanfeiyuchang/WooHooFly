using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class Tile : MonoBehaviour
{
    public GameObject grassTile;
    public GameObject waterTile;
    public TileInfo tileInfo;
    [HideInInspector]
    public int tileBit;
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
                Vector3 position = new Vector3(this.transform.localPosition.x + ((float)(x - 1) * 1 / 3), this.transform.localPosition.y, this.transform.localPosition.z - ((float)(y - 1) * 1 / 3));
                int index = x + y * 3 + 1;
                if (isWaterTile(index))
                    Instantiate(waterTile, position, Quaternion.identity, this.transform);
                else
                    Instantiate(grassTile, position, Quaternion.identity, this.transform);
            }
        }
        tileBit = getBitMaskValue();
    }
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
