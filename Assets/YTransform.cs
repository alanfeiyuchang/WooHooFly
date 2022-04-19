using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YTransform : MonoBehaviour
{
    public Vector3[] endPositions;
    // {(11, 8, -2), (7.5, 4, 2.5) (6.5,6,5) (-0.3,4,3.5) (10,0,-4.5) (9.5,1,-2;
    public Quaternion[] endRotations;
    // = Quaternion.Euler(new Vector3(0,-45,0)) (45) (46.17) (-45) (-134.46) (-136.6);
    // (-0.212 132.38 87.376) (0 -46.147 -87.363)
    public float[] scales;
    private List<Vector3> startPositions = new List<Vector3>();
    private List<Quaternion>startRotations = new List<Quaternion>();
    private List<Vector3> initialSizes = new List<Vector3>();
    public float desiredDuration = 1f;
    private float elapsedTime;

    public void Begin() {
        if (endPositions == null || endPositions.Length != transform.childCount)
            return;
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
            
            StartCoroutine(TransformSeparately());  
    }

    IEnumerator TransformSeparately() {
         // if (endPositions == null || endPositions.Length != transform.childCount)
        //     yield return null;
        for (int i = 0; i < endPositions.Length; i++)
        {
           Debug.Log("start transforming Y " + i);
           yield return StartCoroutine(Transforming(i));
        }           
        yield return null;       
    }

   IEnumerator Transforming(int childIndex) {
        Transform child = transform.GetChild(childIndex);
        while (elapsedTime < desiredDuration) {
            // Debug.Log(elapsedTime);
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / desiredDuration;
            child.localPosition = Vector3.Lerp(startPositions[childIndex], endPositions[childIndex], percentageComplete);
            child.localRotation = Quaternion.Lerp(startRotations[childIndex], endRotations[childIndex], percentageComplete);
            child.localScale = Vector3.Lerp(initialSizes[childIndex], scales[childIndex] * initialSizes[childIndex], percentageComplete );
            yield return null;
        }
        elapsedTime = 0;
        yield return null;
    }
}
