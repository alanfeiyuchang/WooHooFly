using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class SpawnVegetation : MonoBehaviour
{

    [SerializeField]  private SpawnInfo spawnInfo;
    public int repeat;

    public bool showGrass;

    private float minx, minz, maxx, maxz, range;

    
    void Start()
    {
        minx = -0.4f;
        minz = -0.4f;
        range = (float) (0.8 / Sqrt(repeat));
        maxx = minx + range;
        maxz = minz + range;
        if (showGrass) {
            transform.GetChild(4).gameObject.SetActive(true);
        }
        Spawn();
    }


    public void Spawn()
    {
        Spawnee[] spawnees = spawnInfo.spawnees;
        while (repeat > 0)
        {
            float chance = Random.Range(0, 100);
            float pass = 0;
            // Debug.Log(chance);
            foreach (Spawnee info in spawnees)
            {
                pass += info.chance;
                if (chance < pass){
                    Vector3 pos = GetRandomPoint();
                    GameObject x = Instantiate(info.obj, Vector3.zero, info.obj.transform.rotation);
                    x.transform.parent = transform;
                    x.transform.localPosition = pos;              
                    break;               
                }
                
            }
            repeat -= 1;
        }

    }

    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(minx, maxx);
        float y = 0.1f;
        float z = Random.Range(minz, maxz);
        // Debug.Log(range);
        if (maxx + range > 0.4f){
            minx = -0.4f;
            minz += range;
        }
        else{
            minx += range;
        }
        maxx = minx + range;
        maxz = minz + range;
        // Debug.Log("new pos: " + minx + " " + maxx + " | " + minz + " " + maxz);
        // var localPos = new Vector3(x, y, z);
        // return transform.TransformPoint(localPos);
        return new Vector3(x, y, z);
    }

    
}


