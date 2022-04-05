using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnviiorment : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnEnviiorment instanace;
    private void Awake()
    {
        instanace = this;
    }
    [SerializeField] public GameObject waterParent;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grassTop;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnWater(Vector3 _pos)
    {
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(waterParent.transform);
        _empty.transform.position = new Vector3(_pos.x, _pos.y + 0.1f, _pos.z);
        _empty.transform.localScale = new Vector3(1, 1, 1);
    }
    public void spawnSand(Vector3 _pos)
    {
        GameObject _sand = Instantiate(sand);
        _sand.transform.SetParent(transform);
        _sand.transform.position = _pos;
    }
    public void spawnGrass(Vector3 _pos)
    {
        GameObject _grass = Instantiate(grassTop);
        _grass.transform.SetParent(transform);
        _grass.transform.position = new Vector3(_pos.x,_pos.y+0.5f,_pos.z);
    }
}
