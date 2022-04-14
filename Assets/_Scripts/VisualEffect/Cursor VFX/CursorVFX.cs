using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVFX : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particles;

    [SerializeField]
    private Camera mainCamera;

    private Vector3 mousePos;

    // Cursor
    [SerializeField]
    private Texture2D cursor;

    [SerializeField]
    private Texture2D cursorClicked;

    private void Awake()
    {
        ChangeCursor(cursor);
        // Restrict cursor with in the game view
        //Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        particles.Stop();
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // Change cursor to clicked
            ChangeCursor(cursorClicked);

            // Add clicked effect
            mousePos = GetMouseWorldPosition(mainCamera);

            // if the click is not off the map
            if (mousePos.x != -999)
            {
                particles.transform.position = mousePos;
                particles.Play();
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            ChangeCursor(cursor);
        }
    }

    private void ChangeCursor(Texture2D cursorType)
    {
        // Vector2 hotspot = new Vector2(cursorType.width /2, cursorType.height /2);
        Cursor.SetCursor(cursorType, Vector2.zero, CursorMode.Auto);
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
