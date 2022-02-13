using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionSpot : MonoBehaviour
{
    [Tooltip("The transform of where the cube will jump to immediately after the action starts")]
    public Transform GoToTransform;
    public GameObject playerCube;
    public Transform MapTransform;
    [Tooltip("The rotation of the map where the cube can do illusion jump")]
    public Vector3 OnlyAngle;

    public bool CanIllusionJump = false;
    public bool ReadyForJump = false;
    public string direction;
    public static IllusionSpot instance;
    void Update()
    {
        CheckReadyForJump(playerCube.transform);
    }
    void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Check if the player is ideal to do the illusion jump, returns a bool
    /// </summary>
    public bool CheckIllusionJump()
    {
        if (MapTransform.localEulerAngles == OnlyAngle)
        {
            CanIllusionJump = true;
            return true;
        }
        else
        {
            CanIllusionJump = false;
            return false;
        }
    }

    /// <summary>
    /// Do the illustion jump if you can. Takes in playerCube's transform as parameter
    /// </summary>
    /// <param name="playerCubeTransform"></param>
    public void IllusionJump()
    {
        CheckIllusionJump();

        if (CanIllusionJump)
        {
            /*playerCubeTransform = MapTransform;
            CanIllusionJump = false;*/
            //playerCubeTransform = GoToTransform;
            float step = 10000 * Time.deltaTime;
            Debug.Log(GoToTransform.localPosition);
            playerCube.transform.localPosition = Vector3.MoveTowards(playerCube.transform.localPosition, new Vector3(1, (float)2.5, -1), step);
            CanIllusionJump = false;
        }
    }
    public void CheckReadyForJump(Transform playerCubeTransform)
    {
        if (playerCubeTransform.localPosition == new Vector3(-1, (float)0.5, 1))
        {
            ReadyForJump = true;
        }
        else
        {
            ReadyForJump = false;
        }
    }
}
