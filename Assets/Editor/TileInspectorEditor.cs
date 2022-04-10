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
                    if(tile.tileType == TileType.RiverTile)
                        PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/Tile" + tile.tileBit + ".prefab");
                    else if (tile.tileType == TileType.WaterFallTile)
                        PrefabUtility.SaveAsPrefabAsset(t.gameObject, "Assets/Resources/TileSet/WaterFallTile" + tile.tileBit + ".prefab");
                }
            }
        }
    }
}