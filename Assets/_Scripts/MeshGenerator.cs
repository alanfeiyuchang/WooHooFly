using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] public float meshSizeX;
    [SerializeField] public float meshSizeY;
    [SerializeField] public int xSize;
    [SerializeField] public int ySize;
    Mesh mesh;
    Vector3[] verticles;
    int[] triangles;
    Vector2[] uvs;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateFlag();
        updateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GenerateFlag()
    {
        verticles = new Vector3[(xSize + 1) * (ySize + 1)];
        uvs = new Vector2[verticles.Length];
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                verticles[i] = new Vector3(x, y, 0);
                uvs[i] = new Vector2((float)x / xSize, (float)y / ySize);
                i++;
            }
        }
        triangles = new int[xSize * ySize * 6];
        int vert = 0;
        int tris = 0;
        for(int y = 0; y < ySize; y++)
        {
            for(int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 1;
                vert++;
                tris += 6;
            }
            vert++;
        }
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                verticles[i] = new Vector3(x, y, 0);
                i++;
            }
        }
    }
    private void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
