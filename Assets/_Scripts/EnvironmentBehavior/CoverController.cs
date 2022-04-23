using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using WooHooFly.NodeSystem;

public class CoverController : MonoBehaviour
{
    public CubeController playerCube;
    public List<Cover> coverCollection;
    private Camera mainCamera;
    private Camera playerCamera;
    private UniversalAdditionalCameraData cameraData;
    private void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerCamera = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        cameraData = mainCamera.GetUniversalAdditionalCameraData();
    }
    public void UpdateCoverWhenLevelRotate()
    {
        
    }
    public void UpdateCoverWhenCubeMove(bool activeBefore)
    {
        float angle = this.transform.eulerAngles.y;
        foreach (Cover covers in coverCollection)
        {
            if(covers.angle == angle)
            {
                if (activeBefore)
                    PlayerCubeAtCoverNode(covers.ActiveCoverNodesBefore);
                else
                    PlayerCubeAtCoverNode(covers.ActiveCoverNodesAfter);
            }
        }
    }

    private void PlayerCubeAtCoverNode(List<Node> coverNodes)
    {
        if (coverNodes.Contains(playerCube.CurrentNode))
        {
            playerCube.gameObject.layer = LayerMask.NameToLayer("Player");
            if (!cameraData.cameraStack.Contains(playerCamera))
                cameraData.cameraStack.Add(playerCamera);
        }
        else
        {
            playerCube.gameObject.layer = LayerMask.NameToLayer("Level");
            if (cameraData.cameraStack.Contains(playerCamera))
                cameraData.cameraStack.Remove(playerCamera);
        }
    }
}
