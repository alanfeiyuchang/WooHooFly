using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAutoRotate : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float secondPerMove = 1f;
    [SerializeField] private bool move = false;
    [SerializeField] private Transform rotateAround;
    [SerializeField] private Camera camera;
    [SerializeField] private CanvasGroup cg;
    [SerializeField] private float fadeInAndOutTime = 1f;
    private float count = 0f;
    private float beforePositionX;
    private float beforeRotationX;
    private bool recorded = false;
    void Start()
    {
        beforeRotationX = transform.eulerAngles.x;
        Vector3 camTemp = camera.transform.position;
        camTemp.x = this.transform.position.x;
        camera.transform.position = camTemp;
        StartCoroutine(FadeAnim(true));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {
            /*transform.RotateAround(rotateAround.position, -Vector3.forward, 90f * Time.deltaTime / secondPerMove);
            rotateAround.transform.position = new Vector3(transform.position.x, 0.5f, 0f);*/
            if(count == 0f && ! recorded)
            {
                beforePositionX = transform.position.x;
                //beforeRotationX = transform.eulerAngles.x;
                recorded = true;
            }
/*            else if(count<= secondPerMove/2)
            {
                float xPosition = Mathf.Lerp(beforePositionX, beforePositionX + 0.5f, count/(secondPerMove / 2));
                float yPosition = Mathf.Lerp(1, 1.2f, count / (secondPerMove / 2));
                transform.position = new Vector3(xPosition, yPosition, transform.position.z);
                float xRot = Mathf.Lerp(beforeRotationX, beforeRotationX + 45f, count / (secondPerMove / 2));
                transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, transform.eulerAngles.z);
                count += Time.deltaTime;
            }*/
            else if(count <= secondPerMove)
            {
                float xPosition = Mathf.Lerp(beforePositionX, beforePositionX + 1f, count / secondPerMove);
                float yPosition = Mathf.Lerp(1f, 1, count / secondPerMove);
                transform.position = new Vector3(xPosition, yPosition, transform.position.z);
                /* float xRot = Mathf.LerpAngle(beforeRotationX, beforeRotationX + 90f, count / secondPerMove);
                 transform.eulerAngles = new Vector3(xRot, transform.eulerAngles.y, transform.eulerAngles.z);*/
                transform.Rotate(new Vector3(90 / secondPerMove * Time.deltaTime, 0, 0));
                count += Time.deltaTime;
            }
            else
            {
                count = 0f;
                recorded = false;
                beforeRotationX += 90f;
            }

            // camera follow
            Vector3 camTemp = camera.transform.position;
            camTemp.x = this.transform.position.x;
            camera.transform.position = camTemp;
        }
    }

    public void FadeOut()
    {
        StartCoroutine(FadeAnim(false));
    }
    IEnumerator FadeAnim(bool fadeIn)
    {
        if (fadeIn)
        {
            move = false;
            cg.alpha = 1f;
            yield return new WaitForSeconds(2f);
            move = true;
        }
        else
            cg.alpha = 0f;
        float timeElapsed = 0f;
        while (timeElapsed <= fadeInAndOutTime)
        {
            timeElapsed += Time.deltaTime;
            if (fadeIn)
                cg.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeInAndOutTime);
            else
                cg.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInAndOutTime);
            yield return null;
        }
        if (fadeIn)
            cg.alpha = 0f;
        else
        {
            cg.alpha = 1f;
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(0);
        }
            

        
    }
}
