using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using WooHooFly.NodeSystem;

public class MapTransition : MonoBehaviour
{
    public static MapTransition instance;
    private List<GameObject> _fromMapCubes;
    private List<GameObject> _fromMapFlags;
    private GameObject _fromMapPlayerCube;
    private List<GameObject> _toMapCubes;
    private List<GameObject> _toMapFlags;
    private GameObject _toMapPlayerCube;

    private GameObject _dummyLevel;

    private void Awake()
    {
        instance = this;
        foreach (LevelManager levelManager in LevelList)
        {
            levelManager.gameObject.SetActive(false);
        }
        ChangeLevel();
    }
    private LevelManager _fromLevel;
    private LevelManager _toLevel;
    private bool _canDoTransition = false;

    [SerializeField] private int _currentLevel = 0;
    [Tooltip("All the levels in order")]
    [SerializeField] private List<LevelManager> LevelList;
    [Tooltip("Calculate Path on the current Level")]
    [SerializeField] private Graph graph;
    [Tooltip("The total crash time between the start falling of the first cube and last cube")]
    [SerializeField] private float TotalCrashTime = 1.5f;
    [Tooltip("How long each cube will drop")]
    [SerializeField] private float DropTime = 1.5f;
    [Tooltip("How far each cube will drop")]
    [SerializeField] private float DropHeight = 5f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LevelTransition();
        }
    }

    public CubeController GetCurrentCubeControllerScript()
    {
        return LevelList[_currentLevel].PlayerCube.GetComponent<CubeController>();
    }

    public MouseRotation GetCurrentMouseRotationScript()
    {
        return LevelList[_currentLevel].GetComponent<MouseRotation>();
    }

    public void DisableController()
    {
        GetCurrentCubeControllerScript().enabled = false;
        GetCurrentMouseRotationScript().enabled = false;
    }

    public void EnableController()
    {
        GetCurrentCubeControllerScript().enabled = true;
        GetCurrentMouseRotationScript().enabled = true;
    }

    private void ChangeLevel()
    {
        _canDoTransition = false;
        if (_currentLevel < LevelList.Count)
        {
            //set up dummy level
            _dummyLevel = GameObject.Instantiate(LevelList[_currentLevel].gameObject);
            _dummyLevel.SetActive(false);
            _dummyLevel.transform.position = LevelList[_currentLevel].transform.position;
            _dummyLevel.name = "Level " + (_currentLevel + 1);

            _fromLevel = LevelList[_currentLevel];
            _fromMapCubes = _fromLevel.MapCubes;
            _fromMapFlags = _fromLevel.Flags;
            _fromMapPlayerCube = _fromLevel.PlayerCube;

            graph.ReInitPath();
            EnableController();

            _fromLevel.gameObject.SetActive(true);

            if (_currentLevel + 1 < LevelList.Count)
            {
                _toLevel = LevelList[_currentLevel + 1];
                _toMapCubes = _toLevel.MapCubes;
                _toMapFlags = _toLevel.Flags;
                _toMapPlayerCube = _toLevel.PlayerCube;
                _canDoTransition = true;
                _toLevel.gameObject.SetActive(false);
            }
            else
            {
                UIController.instance.NextButton.SetActive(false);
            }
        }
        else
        {
            _canDoTransition = false;
            UIController.instance.NextButton.SetActive(false);
        }
    }


    public void LevelTransition()
    {
        if (_canDoTransition)
        {
            Shuffle(_fromMapCubes);
            Shuffle(_toMapCubes);
            StartCoroutine(LevelCrashOneByOne(DropTime, DropHeight, 1));
        }
    }

    public void RestartLevel()
    {
        _toLevel = _dummyLevel.GetComponent<LevelManager>();
        _toMapCubes = _toLevel.MapCubes;
        _toMapFlags = _toLevel.Flags;
        _toMapPlayerCube = _toLevel.PlayerCube;

        Shuffle(_fromMapCubes);
        Shuffle(_toMapCubes);
        StartCoroutine(LevelCrashOneByOne(DropTime, DropHeight, 0));
    }

    IEnumerator LevelCrashOneByOne(float dropTime, float dropHeight, int plus)
    {
        GameManager.instance.CurrentState = GameManager.GameState.falling;
        //from map drops
        float betweenTime = TotalCrashTime / _fromMapCubes.Count();
        foreach (GameObject go in _fromMapCubes)
        {
            StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, go.transform));
            yield return new WaitForSeconds(betweenTime);
        }
        StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, _fromMapPlayerCube.transform));
        yield return new WaitForSeconds(betweenTime);
        foreach (GameObject flag in _fromMapFlags)
        {
            StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, flag.transform));
        }
        
        yield return new WaitForSeconds(dropTime);
        UIController.instance.StepCount = 0;

        //switch map
        SetPosition(_fromMapCubes, _fromMapFlags, _fromMapPlayerCube, +dropHeight);
        LevelList[_currentLevel] = _dummyLevel.GetComponent<LevelManager>();
        _fromLevel.gameObject.SetActive(false);

        SetPosition(_toMapCubes, _toMapFlags, _toMapPlayerCube, +dropHeight);
        _toLevel.gameObject.SetActive(true);

        //to map appears
        betweenTime = TotalCrashTime / _toMapCubes.Count();
        foreach (GameObject go in _toMapCubes)
        {
            StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, go.transform));
            yield return new WaitForSeconds(betweenTime);
        }
        StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, _toMapPlayerCube.transform));
        yield return new WaitForSeconds(betweenTime);
        foreach (GameObject flag in _toMapFlags)
        {
            StartCoroutine(LevelOneCubeCrash(dropTime, dropHeight, flag.transform));
        }
        yield return new WaitForSeconds(dropTime+0.5f);


        //switch level
        //Destroy(_fromLevel.gameObject);
        DisableController();
        _currentLevel += plus;
        ChangeLevel();
        GameManager.instance.CurrentState = GameManager.GameState.playing;
    }

    IEnumerator LevelOneCubeCrash(float dropTime, float dropHeight, Transform trans)
    {
        float timeElapsed = 0f;
        Vector3 beginPos = trans.position;
        Vector3 endPos = trans.position;
        endPos[1] -= dropHeight;
        while (timeElapsed <= dropTime)
        {
            Vector3 pos = Vector3.Lerp(beginPos, endPos, timeElapsed / dropTime);
            trans.position = pos;
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        Vector3 finalPos = trans.position;
        finalPos[1] = Mathf.Round(finalPos[1]);
        trans.position = finalPos;
    }

    private void SetPosition(List<GameObject> cubes, List<GameObject> flags, GameObject playerCube, float height)
    {
        foreach (GameObject item in cubes)
        {
            Vector3 pos = item.transform.position;
            pos[1] += height;
            item.transform.position = pos;
        }

        foreach (GameObject item in flags)
        {
            Vector3 pos = item.transform.position;
            pos[1] += height;
            item.transform.position = pos;
        }

        Vector3 cubePos = playerCube.transform.position;
        cubePos[1] += height;
        playerCube.transform.position = cubePos;
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
