using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WooHooFly.Colors;
public class FinalTransition : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3[] endPositions;
    // {(11, 8, -2), (7.5, 4, 2.5) (6.5,6,5) (-0.3,4,3.5) (10.5,0,-4) (9.5,1,-0.5;
    public Quaternion[] endRotations;
    // = Quaternion.Euler(new Vector3(0,-45,0)) (45) (46.17) (-45) (-134.46) (-136.6);
    // (-0.212 132.38 87.376) (0 -46.147 -87.363)
    public float[] scales;
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Quaternion> startRotations = new List<Quaternion>();
    private List<Vector3> initialSizes = new List<Vector3>();
    public float desiredDuration = 1f;
    private float elapsedTime;

    public bool allAtOnce;

    private bool transformed;
    void TransformAllAtOnce()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!transformed)
            {
                Begin();
            }
        }
    }

    public void Begin()
    {
        if (endPositions == null || endPositions.Length != transform.childCount)
            return;

        transform.parent.GetChild(1).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);
        for (int i = 0; i < endPositions.Length; i++)
        {
            Transform child = transform.GetChild(i);
            startPositions.Add(child.localPosition);
            startRotations.Add(child.localRotation);
            initialSizes.Add(child.localScale);
            for (int j = 0; j < child.childCount; j++)
            {
                child.GetChild(j).gameObject.SetActive(true);
            }

        }


        //
        //change top color to blue, bottom to red
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform _child = transform.GetChild(i);
            for (int j = 0; j < _child.childCount; j++)
            {
                GameObject _mapCube = _child.GetChild(j).gameObject;
                MapCubeManager _mapManager = _mapCube.GetComponent<MapCubeManager>();
                TileColor _color;
                if (i < 4)
                {
                    _color = TileColor.green;
                }
                else
                {
                    _color = TileColor.red;
                }
                _mapManager.changeAllTileColor(_color);
                _mapManager.changeAllTileType(MapCubeManager.TileType.Changeable);

            }
        }
        Transform _childY = transform.GetChild(transform.childCount - 1);
        for (int i = 0; i < _childY.childCount; i++)
        {
            Transform _child = _childY.GetChild(i);
            for (int j = 0; j < _child.childCount; j++)
            {
                GameObject _mapCube = _child.GetChild(j).gameObject;
                MapCubeManager _mapManager = _mapCube.GetComponent<MapCubeManager>();
                TileColor _color;
                _color = TileColor.red;
                _mapManager.changeAllTileColor(_color);
                _mapManager.changeAllTileType(MapCubeManager.TileType.Changeable);

            }
        }
        StartCoroutine(TransformSeparately());
        transformed = true;
    }

    IEnumerator TransformSeparately()
    {
        // if (endPositions == null || endPositions.Length != transform.childCount)
        //     yield return null;
        for (int i = 0; i < endPositions.Length - 1; i++)
        {
            Debug.Log("start transforming " + i);
            yield return StartCoroutine(Transforming(i));
        }
        YTransform yTransform = transform.GetComponentInChildren<YTransform>();
        yTransform.Begin();
        yield return new WaitForSeconds(6f);

        SceneManager.LoadScene("Credit");

    }

    IEnumerator Transforming(int childIndex)
    {
        Transform child = transform.GetChild(childIndex);
        while (elapsedTime < desiredDuration)
        {
            // Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            child.localPosition = Vector3.Lerp(startPositions[childIndex], endPositions[childIndex], percentageComplete);
            child.localRotation = Quaternion.Lerp(startRotations[childIndex], endRotations[childIndex], percentageComplete);
            child.localScale = Vector3.Lerp(initialSizes[childIndex], scales[childIndex] * initialSizes[childIndex], percentageComplete);
            yield return null;
        }
        elapsedTime = 0;
        yield return null;
    }
}
