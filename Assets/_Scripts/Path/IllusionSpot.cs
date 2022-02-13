using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllusionSpot : MonoBehaviour
{
    [Tooltip("The transform of where the cube will jump to immediately after the action starts")]
    public Transform GoToTransform;

    public Transform MapTransform;
    [Tooltip("The rotation of the map where the cube can do illusion jump")]
    public Vector3 OnlyAngle;

    public bool CanIllusionJump = false;

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
    public void IllusionJump(Transform playerCubeTransform)
    {
        CheckIllusionJump();

        if (CanIllusionJump)
        {
            playerCubeTransform = MapTransform;
            CanIllusionJump = false;
        }
    }
}
