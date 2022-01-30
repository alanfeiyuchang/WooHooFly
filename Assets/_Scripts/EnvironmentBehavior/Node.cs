using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [SerializeField] private float gizmoRadius = 0.1f;
    [SerializeField] private Color defaultGizmoColor = Color.black;
    [SerializeField] private Color selectedGizmoColor = Color.blue;
    [SerializeField] private Color inactiveGizmoColor = Color.gray;

    [SerializeField] private List<Edge> edges = new List<Edge>();

    // check neighbors automatically
    public static Vector3[] neighborDirections =
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(-1f, 0f, 0f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, -1f),
        };

    // draws a sphere gizmo in a different color when selected
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = selectedGizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        // draws a line to each neighbor
        foreach (Edge e in edges)
        {
            if (e.neighbor != null)
            {
                Gizmos.color = (e.isActive) ? selectedGizmoColor : inactiveGizmoColor;
                Gizmos.DrawLine(transform.position, e.neighbor.transform.position);
            }
        }
    }
}
