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
        //base.OnInspectorGUI();
        
        MapCubeManager mapCubeManager = (MapCubeManager)target;
        EditorGUILayout.LabelField("Enable side", EditorStyles.boldLabel);
        mapCubeManager.sideAEnabled = EditorGUILayout.Toggle("Side A Enabled", mapCubeManager.sideAEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideA, mapCubeManager.sideAEnabled);
        mapCubeManager.sideBEnabled = EditorGUILayout.Toggle("Side B Enabled", mapCubeManager.sideBEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideB, mapCubeManager.sideBEnabled);
        mapCubeManager.sideCEnabled = EditorGUILayout.Toggle("Side C Enabled", mapCubeManager.sideCEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideC, mapCubeManager.sideCEnabled);
        mapCubeManager.sideDEnabled = EditorGUILayout.Toggle("Side D Enabled", mapCubeManager.sideDEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideD, mapCubeManager.sideDEnabled);
        mapCubeManager.sideEEnabled = EditorGUILayout.Toggle("Side E Enabled", mapCubeManager.sideEEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideE, mapCubeManager.sideEEnabled);
        mapCubeManager.sideFEnabled = EditorGUILayout.Toggle("Side F Enabled", mapCubeManager.sideFEnabled);
        mapCubeManager.ChangeSideActiveStatus(mapCubeManager.sideF, mapCubeManager.sideFEnabled);
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
