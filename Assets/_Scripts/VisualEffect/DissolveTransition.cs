using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;
public class DissolveTransition : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Material blueDissolve;
    [SerializeField] private Material brownDissolve;
    [SerializeField] private Material greyDissolve;
    [SerializeField] private List<GameObject> mapCube;
    [SerializeField] private float dissolveTime = 3f;
    void Start()
    {
        startDissolve(dissolveTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startDissolve(float time)
    {
        StartCoroutine(dissolve(time));
    }
    private IEnumerator dissolve(float time)
    {
        float timeCount = time;
        while (timeCount >= 0)
        {
            float scale = timeCount / time;
            if (scale < 0.3f)
            {
                foreach (GameObject cube in mapCube)
                {
                    MapCubeManager manager = cube.GetComponent<MapCubeManager>();
                    if (manager.sideAColor == TileColor.green || manager.sideBColor == TileColor.green ||
                        manager.sideCColor == TileColor.green || manager.sideDColor == TileColor.green ||
                        manager.sideEColor == TileColor.green || manager.sideFColor == TileColor.green)
                    {
                        SpawnEnviiorment.instanace.spawnWater(cube.transform.position);
                    }
                    else
                    {
                        SpawnEnviiorment.instanace.spawnSand(cube.transform.position);
                    }
                    cube.SetActive(false);
                }
            }
            blueDissolve.SetFloat("_AlphaThreshold",scale);
            brownDissolve.SetFloat("_AlphaThreshold",scale);
            greyDissolve.SetFloat("_AlphaThreshold",scale);
            timeCount -= Time.deltaTime;
            yield return null;
        }
    }
}
