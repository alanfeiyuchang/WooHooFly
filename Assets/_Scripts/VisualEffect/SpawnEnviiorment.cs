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
    //public GameObject waterPosIndicator;
    [SerializeField] private GameObject sand;
    [SerializeField] private GameObject grassTop;
    //public List<List<Transform>> water_pos_list = new List<List<Transform>>();
    private Transform river;
    void Start()
    {
        //DecomposeWaterPos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void DecomposeWaterPos()
    //{
    //    water_pos_list.Clear();
    //    foreach (Transform side in waterPosIndicator.transform){
    //        List<Transform> sideCollection = new List<Transform>();
    //        foreach (Transform waterTrans in side)
    //        {
    //            sideCollection.Add(waterTrans);
    //        }
    //        water_pos_list.Add(sideCollection);
    //    }
    //}

    public void spawnTile(Transform _tile, Vector3 waterCubeTrans)
    {
        //Debug.Log(_tile.transform.position);
        //spawnGround(_tile.transform);

        // river = this.transform.Find("River");


        int subtileIndex = 0;
        foreach (Transform piece in _tile.transform)
        {
            //if(piece.name == "Green_Tile(Clone)")
            //{
            //    spawnGrass(piece, _tile.transform);
            //}
            if (piece.name == "Blue_Tile(Clone)")
            {
                spawnWater(piece.transform, waterCubeTrans, subtileIndex);
            }
            subtileIndex++;
        }
    }
    public void spawnWater(Vector3 _pos)
    {
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.SetParent(waterParent.transform);
        _empty.transform.position = new Vector3(_pos.x, _pos.y + 0.1f, _pos.z);
        _empty.transform.localScale = new Vector3(1, 1, 1);
    }
    public void spawnWater(Transform tile, Vector3 posToCenter, int subtileIndex)
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

        //_empty.transform.position = new Vector3(_trans.position.x + _trans.localScale.x / 2,
        //    _trans.position.y, _trans.position.z + _trans.localScale.z / 2); 
        GameObject _empty = new GameObject("WaterMark");
        _empty.transform.position = tile.position;
        _empty.transform.SetParent(waterParent.transform);
        _empty.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

        //Debug.Log(posToCenter);
        //if(posToCenter.y > 0)
        //{
        //    // indicator A
        //    Debug.Log("A");
        //    _empty.transform.position = tile.position + water_pos_list[0][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[0][subtileIndex].localScale;
        //}
        //else if (posToCenter.y < 0)
        //{
        //    // indicator B
        //    Debug.Log("B");
        //    _empty.transform.position = tile.position + water_pos_list[1][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[1][subtileIndex].localScale;
        //}
        //else if (posToCenter.z < 0)
        //{
        //    // indicator C
        //    Debug.Log("C");
        //    _empty.transform.position = tile.position + water_pos_list[2][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[2][subtileIndex].localScale;
        //}
        //else if (posToCenter.z > 0){
        //    // indicator D
        //    Debug.Log("D");
        //    _empty.transform.position = tile.position + water_pos_list[3][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[3][subtileIndex].localScale;
        //}
        //else if (posToCenter.x > 0)
        //{
        //    // indicator E
        //    Debug.Log("E");
        //    _empty.transform.position = tile.position + water_pos_list[4][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[4][subtileIndex].localScale;
        //}
        //else if (posToCenter.x < 0)
        //{
        //    // indicator F
        //    Debug.Log("F");
        //    _empty.transform.position = tile.position + water_pos_list[5][subtileIndex].localPosition;
        //    _empty.transform.localScale = water_pos_list[5][subtileIndex].localScale;
        //}
        //switch (posToCenter)
        //{
        //    case Vector3 v when v.Equals(Vector3.up / 2):
        //        // indicator A
        //        _empty.transform.position = tile.position + water_pos_list[0][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[0][subtileIndex].localScale;
        //        break;
        //    case Vector3 v when v.Equals(Vector3.down / 2):
        //        _empty.transform.position = tile.position + water_pos_list[1][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[1][subtileIndex].localScale;
        //        break;
        //    case Vector3 v when v.Equals(Vector3.back / 2):
        //        // indicator C
        //        _empty.transform.position = tile.position + water_pos_list[2][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[2][subtileIndex].localScale;
        //        break;
        //    case Vector3 v when v.Equals(Vector3.forward / 2):
        //        // indicator D
        //        _empty.transform.position = tile.position + water_pos_list[3][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[3][subtileIndex].localScale;
        //        break;
        //    case Vector3 v when v.Equals(Vector3.right / 2):
        //        // indicator E
        //        _empty.transform.position = tile.position + water_pos_list[4][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[4][subtileIndex].localScale;
        //        break;
        //    case Vector3 v when v.Equals(Vector3.left / 2):
        //        // indicator F
        //        Debug.Log("correct");
        //        _empty.transform.position = tile.position + water_pos_list[5][subtileIndex].localPosition;
        //        _empty.transform.localScale = water_pos_list[5][subtileIndex].localScale;
        //        break;
        //    default:
        //        Debug.Log("Wrong Pos" + posToCenter + Vector3.left / 2);
        //        break;
        //}

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
        // _grass.transform.SetParent(transform);
        _grass.transform.SetParent(_trans);
        // _grass.transform.localScale = _trans.localScale;
        _grass.transform.localScale = new Vector3(1,1,1);
        _grass.transform.position = new Vector3(_trans.position.x, _trans.position.y + 0.01f, _trans.position.z); 
        float chance = Random.Range(0, 100);
        SpawnVegetation spawnScript = _grass.GetComponent<SpawnVegetation>();
        if (_trans.tag == "Tree") {
            spawnScript.Spawn();
        }
        else {
            spawnScript.Spawn();
        }
    }
}
