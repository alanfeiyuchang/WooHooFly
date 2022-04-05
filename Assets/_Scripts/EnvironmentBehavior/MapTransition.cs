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
        
    }
    private LevelManager _fromLevel;
    private LevelManager _toLevel;
    private bool _canDoTransition = false;
    private GameObject _dummyFolder;

    public int CurrentLevel = 0;
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
    public int LevelUnlocked = 1;
    [HideInInspector]
    public MouseRotation mouseRotation;
    [HideInInspector]
    public LevelSelectionManager levelSelectionManager;


    private void Start()
    {
        _dummyFolder = transform.GetChild(0).gameObject;
        int levelIndex = 0;
        foreach (LevelManager levelManager in LevelList)
        {
            //make dummy
            levelManager.gameObject.SetActive(false);
            GameObject dummy = Instantiate(levelManager.gameObject);
            dummy.name = "Level " + levelIndex;
            levelManager.Dummy = dummy;
            dummy.transform.SetParent(_dummyFolder.transform);
            dummy.SetActive(false);
            levelIndex++;
        }
        LevelList[CurrentLevel].gameObject.SetActive(true);
        if (LevelList[CurrentLevel].GetComponent<MouseRotation>() != null)
            mouseRotation = LevelList[CurrentLevel].GetComponent<MouseRotation>();
        if (LevelList[0].GetComponent<LevelSelectionManager>() != null)
            levelSelectionManager = LevelList[0].GetComponent<LevelSelectionManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            LevelTransition();
            
        }
    }

    public CubeController GetCurrentCubeControllerScript()
    {
        return LevelList[CurrentLevel].PlayerCube.GetComponent<CubeController>();
    }

    // for camera follow
    public Transform GetCurrentPlayerTransform()
    {
        return LevelList[CurrentLevel].PlayerCube.transform;
    }

    public MouseRotation GetCurrentMouseRotationScript()
    {
        if (LevelList[CurrentLevel].GetComponent<MouseRotation>() == null)
            return null;
        return LevelList[CurrentLevel].GetComponent<MouseRotation>();
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

    public void RedefineNode()
    {
        graph.ReInitPath();
    }

    private void ChangeLevel(int levelIndex)
    {
        
        _canDoTransition = false;
        if (CurrentLevel < LevelList.Count)
        {
            //set up dummy level
            _dummyLevel = GameObject.Instantiate(LevelList[CurrentLevel].Dummy);
            _dummyLevel.SetActive(false);
            _dummyLevel.name = "Level " + (CurrentLevel);
            _dummyLevel.GetComponent<LevelManager>().Dummy = LevelList[CurrentLevel].Dummy;


            _fromLevel = LevelList[CurrentLevel];
            _fromMapCubes = _fromLevel.MapCubes;
            _fromMapFlags = _fromLevel.Flags;
            _fromMapPlayerCube = _fromLevel.PlayerCube;

            graph.ReInitPath();
            //EnableController();

            _fromLevel.gameObject.SetActive(true);

            if (levelIndex < LevelList.Count)
            {
                if (CurrentLevel == levelIndex)
                    _toLevel = _dummyLevel.GetComponent<LevelManager>();
                else
                    _toLevel = LevelList[levelIndex];
                _toMapCubes = _toLevel.MapCubes;
                _toMapFlags = _toLevel.Flags;
                _toMapPlayerCube = _toLevel.PlayerCube;
                _canDoTransition = true;
                _toLevel.gameObject.SetActive(false);
                //_currentLevel = levelIndex;
            }
        }
        else
        {
            _canDoTransition = false;
            UIController.instance.NextButton.SetActive(false);
        }
    }

    public void SelectLevel()
    {
        /*LevelList[0].GetComponent<LevelSelectionManager>().LoadLevelSelection();
        LevelList[0].GetComponent<LevelSelectionManager>().UpdateLevelSelection();*/
        StartCoroutine(LevelCrashOneByOne(DropTime, DropHeight, 1, 0));
    }
    public void LevelTransition()
    {
        StartCoroutine(LevelCrashOneByOne(DropTime, DropHeight, 1, CurrentLevel+1));
    }

    public void LevelTransition(int levelIndex)
    {
        StartCoroutine(LevelCrashOneByOne(DropTime, DropHeight, 1, levelIndex));
    }

    public void RestartLevel()
    {
        LevelTransition(CurrentLevel);
    }

    IEnumerator LevelCrashOneByOne(float dropTime, float dropHeight, int plus, int levelIndex)
    {
        UIController.instance.HideRotateArrow();
        UIController.instance.canShowArrows = true;
        ChangeLevel(levelIndex);
        if (!_canDoTransition)
            yield return null;
        Shuffle(_fromMapCubes);
        Shuffle(_toMapCubes);
        DisableController();
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
        
        yield return new WaitForSeconds(dropTime - 1f);
        
        //switch map
        SetPosition(_fromMapCubes, _fromMapFlags, _fromMapPlayerCube, +dropHeight);
        LevelList[CurrentLevel] = _dummyLevel.GetComponent<LevelManager>();
        _fromLevel.gameObject.SetActive(false);
        Destroy(_fromLevel.gameObject);

        SetPosition(_toMapCubes, _toMapFlags, _toMapPlayerCube, +dropHeight);
        
        _toLevel.gameObject.SetActive(true);
        mouseRotation = _toLevel.GetComponent<MouseRotation>();

        //if it is selection level (level 0) update
        if (levelIndex == 0)
        {
            LevelList[0].GetComponent<LevelSelectionManager>().UpdateLevelSelection();
        }
        CurrentLevel = levelIndex;
        UIController.instance.StepCount = 0;

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

        //Set the camera follow target to the current player cube
        // CameraFollow.instance.SetCameraTarget(_toMapPlayerCube.transform);

        //switch level
        EnableController();
        
        UIController.instance.NextButton.SetActive(true);
        if (levelIndex + 1 >= LevelList.Count)
        {
            UIController.instance.NextButton.SetActive(false);
        }
        
        GameManager.instance.CurrentState = GameManager.GameState.playing;
        UIController.instance.stepCounterActive = true;

        // Reset Analytics timers and pass level info
        //Debug.Log("[Analytics] Level "+ CurrentLevel + " started");
        GameManager.instance.resetAnalyticsTimer(CurrentLevel);

        UIController.instance.ShowRotateArrow();
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

            //prevent from drop too low
            if (trans.position.y <= endPos.y)
                yield break;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        /*Vector3 finalPos = trans.position;
        finalPos[1] = Mathf.Round(finalPos[1]);*/
        trans.position = endPos;
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
