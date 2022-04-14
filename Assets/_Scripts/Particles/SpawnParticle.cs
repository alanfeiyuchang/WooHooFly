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
        particles.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("ok");
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = GetMouseWorldPosition(mainCamera);
            Debug.Log(mousePos);
            if (mousePos.x != -999)
            {
                particles.transform.position = mousePos;
                particles.Play();
            }
            else
            {
                Debug.Log("no hit");
            }

        }
    }

    public static Vector3 GetMouseWorldPosition(Camera worldCamera)
    {
        Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        return new Vector3(-999, -999, -999);
    }
}
