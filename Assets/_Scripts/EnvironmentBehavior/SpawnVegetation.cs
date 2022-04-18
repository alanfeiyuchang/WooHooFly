using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class SpawnVegetation : MonoBehaviour
{

    public float size;
    public float clearZone;

    [SerializeField]  private SpawnInfo spawnInfo;

    public int repeat;

    public bool showGrass;

    public bool showSingle;

    private float minx, minz, maxx, maxz, range;

    
    void Start()
    {
        // repeat = Random.Range(1, repeat+1);
        // Debug.Log(repeat);
        size -= clearZone * 2;
        minx = -size / 2;
        minz = -size / 2;
        range = (float) (size / Sqrt(repeat));
        maxx = minx + range;
        maxz = minz + range;

        // Spawn();
    }


    public void Spawn(bool isTreeTile)
    {
        if (showGrass) {
            transform.GetChild(4).gameObject.SetActive(true);
        }

        if (showSingle) {
            GameObject[] trees = spawnInfo.Trees;
            GameObject[] flowers = spawnInfo.Flowers;
            // this position spawns a tree
            if (isTreeTile) {
                float chance = Random.Range(0, 100);
                float range = 100 / trees.Length;
                float start = 0;
                int index = 0;
                while (start < 100)
                {
                    if (chance < start + range) {
                        GameObject tree = trees[index];
                        Vector3 pos = new Vector3(0,0,-0.2f);
                        GameObject x = Instantiate(tree, Vector3.zero, tree.transform.rotation);
                                x.transform.parent = transform;
                                x.transform.localPosition = pos;
                                x.transform.localScale = tree.transform.localScale;
                        break;
                    }
                    else {
                        index += 1;
                        start += range;
                    }
                   
                }
                return; 
            }         
            // this position does not spawn a tree
            else {
                GameObject flower = flowers[0];
                Vector3 pos = new Vector3(0,2,0);
                GameObject x = Instantiate(flower, Vector3.zero, flower.transform.rotation);
                        x.transform.parent = transform;
                        x.transform.localPosition = pos;
                        x.transform.localScale = new Vector3(1,1,1);
                return; 
            }
        }
    

        // while (repeat > 0)
        // {
        //     float chance = Random.Range(0, 100);
        //     float pass = 0;
        //     // Debug.Log(chance);
        //     foreach (Spawnee info in spawnees)
        //     {
        //         pass += info.chance;
        //         if (chance < pass){
        //             Vector3 pos = GetRandomPoint();
        //             GameObject x = Instantiate(info.obj, Vector3.zero, info.obj.transform.rotation);
        //             x.transform.parent = transform;
        //             x.transform.localPosition = pos;              
        //             break;               
        //         }
                
        //     }
        //     repeat -= 1;
        // }

    }

    public Vector3 GetRandomPoint()
    {
        float x = Random.Range(minx, maxx);
        float y = 0;
        if (showGrass)
            y += 0.1f;
        float z = Random.Range(minz, maxz);
        // Debug.Log(range);
        if (maxx + range > size / 2){
            minx = -size/2;
            if (maxz + range > size / 2){
                minz = -size/2;
            }
            else {
                minz += range;
            }
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


