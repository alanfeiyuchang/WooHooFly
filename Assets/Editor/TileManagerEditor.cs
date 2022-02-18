using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileManager))]
public class TileManagerEditor : Editor
{
    private void OnEnable()
    {

    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        /*TileManager tileManager = (TileManager)target;
        EditorGUILayout.LabelField("Tile color is: ",
           tileManager.MapColor);*/
    }

}
