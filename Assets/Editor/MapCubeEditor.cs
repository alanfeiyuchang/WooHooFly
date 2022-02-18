using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapCubeManager))]
public class MapCubeEditor : Editor
{
    
    private void OnEnable()
    {
       
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapCubeManager mapCubeManager = (MapCubeManager)target;
        //mapCubeManager.sideAEnabled = EditorGUILayout.Toggle("Side A Enabled", mapCubeManager.sideAEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideA, mapCubeManager.sideAType);
        //mapCubeManager.sideBEnabled = EditorGUILayout.Toggle("Side B Enabled", mapCubeManager.sideBEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideB, mapCubeManager.sideBType);
        //mapCubeManager.sideCEnabled = EditorGUILayout.Toggle("Side C Enabled", mapCubeManager.sideCEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideC, mapCubeManager.sideCType);
        //mapCubeManager.sideDEnabled = EditorGUILayout.Toggle("Side D Enabled", mapCubeManager.sideDEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideD, mapCubeManager.sideDType);
        //mapCubeManager.sideEEnabled = EditorGUILayout.Toggle("Side E Enabled", mapCubeManager.sideEEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideE, mapCubeManager.sideEType);
        //mapCubeManager.sideFEnabled = EditorGUILayout.Toggle("Side F Enabled", mapCubeManager.sideFEnabled);
        mapCubeManager.ChangeTileType(mapCubeManager.sideF, mapCubeManager.sideFType);

        mapCubeManager.changeTileColor(mapCubeManager.sideA, mapCubeManager.sideAColor);
        mapCubeManager.changeTileColor(mapCubeManager.sideB, mapCubeManager.sideBColor);
        mapCubeManager.changeTileColor(mapCubeManager.sideC, mapCubeManager.sideCColor);
        mapCubeManager.changeTileColor(mapCubeManager.sideD, mapCubeManager.sideDColor);
        mapCubeManager.changeTileColor(mapCubeManager.sideE, mapCubeManager.sideEColor);
        mapCubeManager.changeTileColor(mapCubeManager.sideF, mapCubeManager.sideFColor);

        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideA, mapCubeManager.NodeAEnabled);
        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideB, mapCubeManager.NodeBEnabled);
        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideC, mapCubeManager.NodeCEnabled);
        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideD, mapCubeManager.NodeDEnabled);
        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideE, mapCubeManager.NodeEEnabled);
        mapCubeManager.ChangeNodeActiveStatus(mapCubeManager.sideF, mapCubeManager.NodeFEnabled);
    }
    public void OnSceneGUI()
    {
        MapCubeManager mapCubeManager = (MapCubeManager)target;
        Handles.Label(mapCubeManager.sideA.transform.position, "Side A");
        Handles.Label(mapCubeManager.sideB.transform.position, "Side B");
        Handles.Label(mapCubeManager.sideC.transform.position, "Side C");
        Handles.Label(mapCubeManager.sideD.transform.position, "Side D");
        Handles.Label(mapCubeManager.sideE.transform.position, "Side E");
        Handles.Label(mapCubeManager.sideF.transform.position, "Side F");
    }
}
