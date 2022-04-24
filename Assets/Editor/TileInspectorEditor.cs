using UnityEditor;
using UnityEngine;

namespace TileSystem
{
    [CustomEditor(typeof(Tile))]
    public class TileInspectorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Tile tile = (Tile)target;
            if (GUILayout.Button("Generate Tile"))
            {
                for (var i = tile.transform.childCount; i > 0; i--)
                {
                    DestroyImmediate(tile.transform.GetChild(0).gameObject);
                }

                tile.GenerateTile();

                Transform[] transforms = Selection.transforms;
                foreach (Transform t in transforms)
                {
                    switch (tile.tileType)
                    {
                        case TileType.RiverTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/RiverTiles/River_Tile" + tile.tileBit + ".prefab");
                            break;
                        case TileType.WaterFallTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/WaterFallTiles/WaterFall_Tile" + tile.tileBit + ".prefab");
                            break;
                        case TileType.LavaTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/LavaTiles/LavaTile_Tile" + tile.tileBit + ".prefab");
                            break;
                        case TileType.LavaFallTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/LavaFallTiles/LavaFallTile_Tile" + tile.tileBit + ".prefab");
                            break;
                        case TileType.GrassTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/EmptyTiles/GrassTile.prefab");
                            break;
                        case TileType.LavaRockTile:
                            PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/EmptyTiles/RockTile.prefab");
                            break;
                    }

                }
            }
        }
    }
}