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

    public void UpdateCoverWhenLevelRotate(Vector3 predictNextAngle)
    {
        float preditctAngle = predictNextAngle.y;
        if (preditctAngle < 0)
            preditctAngle += 360;
        foreach (Cover covers in coverCollection)
        {
            if(covers.angle == preditctAngle)
            {
                if (covers.PlayerLayerNode.Contains(playerCube.CurrentNode))
                {
                    ChangeToPlayerLayer();
                    return;
                }
            }
        }
        ChangeToLevelLayer();
    }
    public void UpdateCoverWhenCubeMove(Node startNode, Node endNode)
    {
        float angle = this.transform.eulerAngles.y;
        foreach (Cover covers in coverCollection)
        {
            if(covers.angle == angle)
            {
                if (covers.BothLayerNode.Contains(startNode))
                {
                    // playerCube layer depends on endNode layer
                    if (covers.PlayerLayerNode.Contains(endNode))
                    {
                        ChangeToPlayerLayer();
                    }
                    else
                    {
                        ChangeToLevelLayer();
                    }
                }
                else
                {
                    // playerCube layer depends on startNode layer
                    if (covers.PlayerLayerNode.Contains(endNode) || covers.PlayerLayerNode.Contains(startNode))
                    {
                        ChangeToPlayerLayer();
                    }
                    else
                    {
                        ChangeToLevelLayer();
                    }
                }
            }
        }
    }

    private void ChangeToPlayerLayer()
    {
        if (!cameraData.cameraStack.Contains(playerCamera))
            cameraData.cameraStack.Add(playerCamera);
    }

    private void ChangeToLevelLayer()
    {
        if (cameraData.cameraStack.Contains(playerCamera))
            cameraData.cameraStack.Remove(playerCamera);
    }
}
