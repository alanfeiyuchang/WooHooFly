using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.Colors;
using Bitgem.VFX.StylisedWater;
public class DissolveTransition : MonoBehaviour
{
    public static DissolveTransition instance;
    // Start is called before the first frame update
    [SerializeField] private Material blueDissolve;
    [SerializeField] private Material brownDissolve;
    [SerializeField] private Material greyDissolve;
    [SerializeField] private Material WaterMaterial;
    [SerializeField] private Material GroundMaterial;
    private List<GameObject> mapCube;
    [SerializeField] private float dissolveTime = 3f;
    private void Awake()
    {
        instance = this;
        //GroundMaterial.color = new Color(1, 1, 1, 0);
        //WaterMaterial.SetFloat("_Alpha", 1);
        //GroundMaterial.SetFloat("_Surface", 1);
    }
    void Start()
    {
        //startDissolve(dissolveTime);
        mapCube = MapTransition.instance.GetCurrentLevel().MapCubes;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //startDissolve(dissolveTime);
        }
    }
    private void OnApplicationQuit()
    {
        GroundMaterial.color = new Color(1, 1, 1, 1);
        WaterMaterial.SetFloat("_Alpha", 0);
        GroundMaterial.SetFloat("_Surface", 1);
        blueDissolve.SetFloat("_AlphaThreshold", 1);
        brownDissolve.SetFloat("_AlphaThreshold", 1);
        greyDissolve.SetFloat("_AlphaThreshold", 1);
    }
    public void resetMaterail()
    {
        GroundMaterial.color = new Color(1, 1, 1, 1);
        WaterMaterial.SetFloat("_Alpha", 0);
        GroundMaterial.SetFloat("_Surface", 1);
        blueDissolve.SetFloat("_AlphaThreshold", 1);
        brownDissolve.SetFloat("_AlphaThreshold", 1);
        greyDissolve.SetFloat("_AlphaThreshold", 1);
    }
    public void startDissolve(float time)
    {
        StartCoroutine(dissolve(time));
    }
    private IEnumerator dissolve(float time)
    {
        float timeCount = time;
        foreach (GameObject cube in mapCube)
        {
            MapCubeManager manager = cube.GetComponent<MapCubeManager>();
            SpawnEnviiorment.instance.spawnSand(cube.transform.position);
            if (manager.sideAColor == TileColor.green || manager.sideBColor == TileColor.green ||
                manager.sideCColor == TileColor.green || manager.sideDColor == TileColor.green ||
                manager.sideEColor == TileColor.green || manager.sideFColor == TileColor.green)
            {
                //SpawnEnviiorment.instanace.spawnWater(cube.transform.position);
                //SpawnEnviiorment.instanace.spawnWater(cube.transform.position);
            }
            else
            {
                //SpawnEnviiorment.instanace.spawnGrass(cube.transform.position);
            }
        }
        //SpawnEnviiorment.instanace.spawnWater(mapCube[0].transform.position);
        //SpawnEnviiorment.instanace.waterParent.transform.localScale = new Vector3(1, 0.1f, 1);
        while (timeCount >= 0)
        {
            float scale = timeCount / time;
            blueDissolve.SetFloat("_AlphaThreshold",scale);
            brownDissolve.SetFloat("_AlphaThreshold",scale);
            greyDissolve.SetFloat("_AlphaThreshold",scale);
            GroundMaterial.color = new Color (1, 1, 1, 1-scale);
            WaterMaterial.SetFloat("_Alpha", scale);
            // Debug.Log(WaterMaterial.GetFloat("_Alpha"));
            timeCount -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        /*foreach (GameObject cube in mapCube)
        {
            cube.SetActive(false);
        }*/
        GroundMaterial.SetFloat("_Surface", 0);
    }
}
