using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;

    [SerializeField]
    private Camera mainCamera;

    private Vector3 mousePos;

    void Start()
    {
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ok");
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Mouse3D.GetMouseWorldPosition(mainCamera);
            particles.SetActive(true);
            particles.transform.position = mousePos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            particles.SetActive(false);
        }
    }
}
