using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    // public Vector3 vector;
    private Animator animator;
    private Vector3 upRotation = new Vector3(-54.9f, 115, 164.1f);
    private Vector3 leftToDownRightRotation =  new Vector3(-33.84f, 158.2f, 75.3f);
    private Vector3 leftToUpRightRotation = new Vector3(-35.84f, 235.0f, 19.024f); 
    private Vector3 rightToUpLeftRotation = new Vector3(102.45f, -24.1f, 213.3f); 
    private Vector3 rightToDownLeftRotation = new Vector3(65.9f, -191.0f, 50.0f); 
    public bool isUpAndDown;
    public bool isLeftToRight;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame

    public void Rotate(bool left)
    {
        if (left && !isLeftToRight) {
            animator.SetTrigger("left2right");
            isLeftToRight = true;
        }
        if (!left && isLeftToRight) {
            animator.SetTrigger("right2left");
            isLeftToRight = false;
        }
    }

    public void OnRotateMap(string direction)
    {
        Debug.Log("rotate to " + direction);
        switch (direction)
        {
            case "L2DR":
                transform.localEulerAngles = leftToDownRightRotation;
                Rotate(true);
                break;
            case "R2UL":
                transform.localEulerAngles = rightToUpLeftRotation;
                Rotate(true);
                break;
            case "L2UR":
                transform.localEulerAngles = leftToUpRightRotation;
                Rotate(false);
                break;
            case "R2DL":
                transform.localEulerAngles = rightToDownLeftRotation;
                Rotate(false);
                break;
            // case "up":
            //     transform.localEulerAngles = upRotation;
            //     animator.SetTrigger("up");
            //     break;
        }
    }
}
