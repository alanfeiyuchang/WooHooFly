using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionSpot : MonoBehaviour
{
    [Tooltip("The transform of where the cube will jump to immediately after the action starts")]
    public Transform GoToTransform;
    public GameObject playerCubeTransform;
    public Transform MapTransform;
    [Tooltip("The rotation of the map where the cube can do illusion jump")]
    public Vector3 OnlyAngle;
    public string direction;
    public bool CanIllusionJump = false;
    public bool ReadyForJump = false;
    public static IllusionSpot instance;
    void Update()
        {
            CheckReadyForJump(playerCubeTransform.transform);
        }
    void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Check if the player is ideal to do the illusion jump
    /// </summary>
    public void CheckIllusionJump()
    {
        if (MapTransform.localEulerAngles == OnlyAngle)
        {
            CanIllusionJump = true;
        }
        else
        {
            CanIllusionJump = false;
        }
    }

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
            playerCubeTransform.transform.localPosition = Vector3.MoveTowards(playerCubeTransform.transform.localPosition, new Vector3(1,(float)2.5,-1), step);
            CanIllusionJump = false;
        }
    }
    public void CheckReadyForJump(Transform playerCubeTransform)
    {
        if(playerCubeTransform.localPosition == new Vector3(-1, (float)0.5, 1))
        {
            ReadyForJump = true;
        }
        else
        {
            ReadyForJump = false;
        }
    }
}
