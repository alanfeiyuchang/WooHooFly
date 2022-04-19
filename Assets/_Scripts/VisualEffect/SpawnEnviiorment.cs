using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnEnviiorment : MonoBehaviour
{
    // Start is called before the first frame update
    public static SpawnEnviiorment instance;
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] public GameObject waterParent;
    public GameObject waterPosIndicator;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grassTop;
    public List<List<Transform>> water_pos_list = new List<List<Transform>>();
    private Transform river;
    void Start()
    {
        DecomposeWaterPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DecomposeWaterPos()
    {
        water_pos_list.Clear();
        foreach (Transform side in waterPosIndicator.transform){
            List<Transform> sideCollection = new List<Transform>();
            foreach (Transform waterTrans in side)
            {
                sideCollection.Add(waterTrans);
            }
            water_pos_list.Add(sideCollection);
        }
    }

    public void spawnTile(GameObject _tile, Transform side)
    {
        //Debug.Log(_tile.transform.position);
        //spawnGround(_tile.transform);

        river = this.transform.Find("River");

        int subtileCount = 0;
        foreach (Transform piece in _tile.transform)
        {
            //if(piece.name == "Green_Tile(Clone)")
            //{
            //    spawnGrass(piece, _tile.transform);
            //}
            if(piece.name == "Water_Tile")
            {
                spawnWater(_tile.transform, subtileCount, side);
            }
            if(piece.name != "Blue_Tile(Clone)")
            subtileCount++;
        }
    }
    public void spawnWater(Vector3 _pos)
    {
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(waterParent.transform);
        _empty.transform.position = new Vector3(_pos.x, _pos.y + 0.1f, _pos.z);
        _empty.transform.localScale = new Vector3(1, 1, 1);
    }
    public void spawnWater(Transform tile, int subtileIndex, Transform side)
    {
        /*if (river == null)
        {
            river = Instantiate(waterParent, _trans.position, _trans.rotation, this.transform).transform;
            river.gameObject.name = "River";
        }

        GameObject _water = Instantiate(waterParent, _trans.position, _trans.rotation, river);
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(_water.transform);
        _empty.transform.localPosition = new Vector3(0, 0, 0);
        _empty.transform.localScale = new Vector3(1, 1, 1);
        GameObject _empty2 = new GameObject("WaterMark");
        _empty2.transform.SetParent(_water.transform);
        _empty2.transform.localPosition = new Vector3(0, 0, 0);
        _empty2.transform.localScale = new Vector3(1, 1, 1);
        _water.transform.localScale = _trans.localScale;*/
        //GameObject _water = Instantiate(waterParent, _trans.position, _trans.rotation, transform);

        /*GameObject _empty2 = new GameObject("WaterMark");
        _empty2.transform.SetParent(waterParent.transform);
        _empty2.transform.position = _trans.position;
        _empty2.transform.localScale = _trans.localScale;*/
        //GameObject _water = Instantiate(waterParent, _trans.position, _trans.rotation, transform);
        //GameObject _empty = new GameObject("WaterMark");
        //_empty.transform.SetParent(river.transform);
        //_empty.transform.position = _trans.position;
        //_empty.transform.localScale = new Vector3(1, 1, 1);
        //GameObject _empty2 = new GameObject("WaterMark");
        //_empty2.transform.SetParent(river.transform);
        //_empty2.transform.position = _trans.position;
        //_empty2.transform.localScale = new Vector3(1, 1, 1);
        //river.transform.localScale = new Vector3 (_trans.localScale.x, _trans.localScale.y / 10, _trans.localScale.z );

        GameObject _empty = new GameObject("WaterMark");

        //_empty.transform.position = new Vector3(_trans.position.x + _trans.localScale.x / 2,
        //    _trans.position.y, _trans.position.z + _trans.localScale.z / 2); 


        _empty.transform.SetParent(waterParent.transform);

        switch (side.gameObject.name)
        {
            case "Side A":
                _empty.transform.position = tile.position + water_pos_list[0][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[0][subtileIndex].localScale;
                //_empty.transform.position = _trans.position;
                //_empty.transform.localScale = _trans.localScale;
                break;
            case "Side B":
                _empty.transform.position = tile.position + water_pos_list[1][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[1][subtileIndex].localScale;
                //_empty.transform.position = _trans.position;
                //_empty.transform.localScale = _trans.localScale;
                break;
            case "Side C":
                _empty.transform.position = tile.position + water_pos_list[2][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[2][subtileIndex].localScale;
                //_empty.transform.position = _trans.position;
                //_empty.transform.position += _trans.transform.up * -0.2f;
                //_empty.transform.localScale = new Vector3(_trans.localScale.x, _trans.localScale.z, _trans.localScale.y);
                break;
            case "Side D":
                _empty.transform.position = tile.position + water_pos_list[3][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[3][subtileIndex].localScale; 
                //_empty.transform.position = _trans.position;
                //_empty.transform.localScale = new Vector3(_trans.localScale.x, _trans.localScale.z, _trans.localScale.y);
                break;
            case "Side E":
                _empty.transform.position = tile.position + water_pos_list[4][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[4][subtileIndex].localScale;
                //_empty.transform.position = _trans.position;
                //_empty.transform.localScale = new Vector3(_trans.localScale.y, _trans.localScale.z, _trans.localScale.x);
                break;
            case "Side F":
                _empty.transform.position = tile.position + water_pos_list[5][subtileIndex].localPosition;
                _empty.transform.localScale = water_pos_list[5][subtileIndex].localScale;
                //_empty.transform.position = _trans.position;
                //_empty.transform.position += _trans.transform.up * -0.2f;
                //_empty.transform.localScale = new Vector3(_trans.localScale.y, _trans.localScale.z, _trans.localScale.x);
                break;
        }

        //Debug.Log(_trans.localScale + " ==== " + _trans.lossyScale);

        //_empty.transform.localScale = new Vector3(_trans.localScale.x, 0.4f, _trans.localScale.z);
    }


    public void DestroyWater()
    {
        foreach (Transform child in waterParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void spawnGround(Transform _trans)
    {
        GameObject _sand = Instantiate(sand);
        _sand.transform.SetParent(transform);
        _sand.transform.position = _trans.position;
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
    public void spawnGrass(Transform _trans, Transform parent)
    {
        GameObject _grass = Instantiate(grassTop);
        _grass.transform.SetParent(transform);
        _grass.transform.localScale = _trans.localScale;
        _grass.transform.position = new Vector3(_trans.position.x, _trans.position.y + 0.01f, _trans.position.z); 
    }
}
