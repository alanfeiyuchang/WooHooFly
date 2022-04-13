using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticle : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private Camera mainCamera;

    private Vector3 mousePos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ok");
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Mouse3D.GetMouseWorldPosition(mainCamera);
            particles.transform.position = mousePos;
            particles.Play();
        }
    }
}
