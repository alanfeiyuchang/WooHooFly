using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MousePosition3D : MonoBehaviour
{
    private ParticleSystem particle;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private GameObject particles;

    void Start()
    {
        particles.SetActive(false);
    }
    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Debug.Log(raycastHit.point);
            transform.position = raycastHit.point;
        }
    }
}
