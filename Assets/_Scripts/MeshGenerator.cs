using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] public int vertAmountX;
    [SerializeField] public int vertAmountY;
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
    private void GenerateTest()
    {
        verticles = new Vector3[4];
        verticles[0] = new Vector3(0.5f, 0.5f, 0);
        verticles[1] = new Vector3(-0.5f, 0.5f, 0);
        verticles[2] = new Vector3(0.5f, -0.5f, 0);
        verticles[3] = new Vector3(-0.5f, -0.5f, 0);
        triangles = new int[6]
        {
            1,3,2,2,1,4
        };
    }
    private void GenerateFlag()
    {
        verticles = new Vector3[(vertAmountX + 1) * (vertAmountY + 1)];
        uvs = new Vector2[verticles.Length];
        for (int i = 0, y = 0; y <= vertAmountY; y++)
        {
            for (int x = 0; x <= vertAmountX; x++)
            {
                float vertX = x * xSize / vertAmountX;
                float vertY = y * ySize / vertAmountY;
                verticles[i] = new Vector3(vertX, vertY, 0);
                uvs[i] = new Vector2((float)x / vertAmountX, (float)y / vertAmountY);
                i++;
            }
        }
        triangles = new int[vertAmountX * vertAmountY * 6];
        int vert = 0;
        int tris = 0;
        for(int y = 0; y < vertAmountY; y++)
        {
            for(int x = 0; x < vertAmountX; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + vertAmountX + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + vertAmountX + 1;
                triangles[tris + 5] = vert + vertAmountX + 2;
                vert++;
                tris += 6;
            }
            vert++;
        }
 
    }
    private void updateMesh()
    {
        mesh.Clear();
        mesh.vertices = verticles;
        mesh.triangles = triangles;
        //mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}
