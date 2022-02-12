using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public enum Direction
    {
        Forward, Backward, Left, Right
    }
    public class compassDirection
    {
        public Vector3 forward = Vector3.forward;
        public Vector3 up = Vector3.up;
        public Vector3 down = Vector3.down;
        public Vector3 right = Vector3.right;
        public Direction state = Direction.Forward; // map rotation related to compass
    }

    public compassDirection compass = new compassDirection();
    private void Start()
    {
        UpdateOrientation();
    }
    public Vector3 forwardPivot
    {
        get { return compass.forward / 2 - compass.up / 2; }
    }

    public Vector3 forwardAxis
    {
        get{ return compass.right; }
    }

    public Vector3 rightPivot
    {
        get { return compass.right / 2 - compass.up / 2; }
    }

    public Vector3 rightAxis
    {
        get { return -compass.forward; }
    }

    public Vector3 backwardPivot
    {
        get { return -compass.forward / 2 - compass.up / 2; }
    }

    public Vector3 backwardAxis
    {
        get { return -compass.right; }
    }

    public Vector3 leftPivot
    {
        get { return -compass.right / 2 - compass.up / 2; }
    }

    public Vector3 leftAxis
    {
        get { return compass.forward; }
    }

    public Vector3 upPivot
    {
        get { return compass.up; }
    }
    public Vector3 downPivot
    {
        get { return compass.down; }
    }
    public void UpdateOrientation()
    {
        float angle = Vector3.Angle(this.transform.forward, Vector3.forward);
        float angle2 = Vector3.Angle(this.transform.forward, Vector3.right);
        angle = (angle2 > 90) ? 360 - angle : angle;

        if (angle >= 0 && angle < 90)
        {
            compass.forward = -this.transform.right;
            compass.right = this.transform.forward;
            compass.state = Direction.Forward;
        }
        else if (angle >= 90 && angle < 180)
        {
            compass.forward = -this.transform.forward;
            compass.right = -this.transform.right;
            compass.state = Direction.Right;
        }
        else if (angle >= 180 && angle < 270)
        {
            compass.forward = this.transform.right;
            compass.right = -this.transform.forward;
            compass.state = Direction.Backward;
        }
        else if (angle >= 270 && angle < 360)
        {
            compass.forward = this.transform.forward;
            compass.right = this.transform.right;
            compass.state = Direction.Left;
        }
}
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; 
        Gizmos.DrawLine(transform.position, transform.position + compass.forward * 5);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + compass.right * 5);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + compass.up * 5);
    }
}
