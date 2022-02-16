using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MapTransition : MonoBehaviour
{
    public static MapTransition instance;
    public List<GameObject> mapCubes;
    private void Awake()
    {
        instance = this;
    }
    public LevelManager FromLevel;
    public LevelManager ToLevel;
    [SerializeField] private float InBetweenTime = 0.1f;
    [SerializeField] private float DropTime = 1.5f;
    [SerializeField] private float DropHeight = 5f;

    private void Start()
    {
        FromLevelCrash();
    }

    private void FromLevelCrash()
    {
        mapCubes = FromLevel.MapCubes;
        Shuffle(mapCubes);
        StartCoroutine(FromLevelCrashOneByOne(InBetweenTime, DropTime, DropHeight));
    }

    IEnumerator FromLevelCrashOneByOne(float betweenTime, float dropTime, float dropHeight)
    {
        foreach (GameObject go in mapCubes)
        {
            StartCoroutine(FromLevelOneCubeCrash(dropTime, dropHeight, go.transform));
            yield return new WaitForSeconds(betweenTime);
        }
        
    }

    IEnumerator FromLevelOneCubeCrash(float dropTime, float dropHeight, Transform trans)
    {
        float timeElapsed = 0f;
        Transform beginTrans = trans;
        Vector3 endPos = trans.position;
        endPos[1] -= dropHeight;
        while (timeElapsed <= dropTime)
        {
            Vector3 pos = Vector3.Lerp(beginTrans.position, endPos, timeElapsed / dropTime);
            trans.position = pos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Shuffle(List<GameObject> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
