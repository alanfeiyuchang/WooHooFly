using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeOnTheWall : MonoBehaviour
{
    [System.Serializable]
    public class Direction
    {
        public bool forward;
        public bool backward;
        public bool left;
        public bool right;
    }
    [System.Serializable]
    public class currentTileInfo
    {
        public Direction edge;
        public Direction wall;
        public bool IsEdgeCube;
        public bool IsConnectCube;
        public bool OnTheWall;
        public bool DownWall;
    }

    public float speed = 300;
    private bool moving = false;

    public Compass compassInfo;
    public currentTileInfo tileInfo = new currentTileInfo();

    ///need to be replace with TileInfo in future
    [HideInInspector]
    public bool[] edgeDirection = new bool[4];
    [HideInInspector]
    public bool[] connectDirection = new bool[4];
    [HideInInspector]
    public bool IsEdgeCube;
    [HideInInspector]
    public bool IsConnectCube;
    ///need to be replace with TileInfo in future
    [HideInInspector]
    public bool OnTheWall;
    [HideInInspector]
    public bool DownWall;
    public MapBaseRotation mapBaseRotation;
    //public GameObject mapmapBaseRotation;

    public static cubeOnTheWall instance;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Roll(Vector3 rotationPointOffset, Vector3 rotationAxis)
    {
        float remainingAngle = 90;
        moving = true;
        Vector3 rotationPoint = this.transform.position + rotationPointOffset;

        while (remainingAngle > 0)
        {
            float rotationAngle = Mathf.Min(Time.deltaTime * speed, remainingAngle);
            transform.RotateAround(rotationPoint, rotationAxis, rotationAngle);
            remainingAngle -= rotationAngle;

            GameManager.instance.CurrentState = GameManager.GameState.rotating;
            yield return null;
        }

        GameManager.instance.CurrentState = GameManager.GameState.playing;
        UIController.instance.AddStep();
        moving = false;
    }

    /*
        Update bottom tile connection information based on the world y-axis rotation 
    */
    private void UpdateTileInfo()
    {
        /*need to be polished in the future*/
        if (moving)
            return;
        if (compassInfo.compass.state == Compass.Direction.Forward)
        {
            tileInfo.edge.forward = edgeDirection[2];
            tileInfo.edge.backward = edgeDirection[3];
            tileInfo.edge.left = edgeDirection[1];
            tileInfo.edge.right = edgeDirection[0];

            tileInfo.wall.forward = connectDirection[2];
            tileInfo.wall.backward = connectDirection[3];
            tileInfo.wall.left = connectDirection[1];
            tileInfo.wall.right = connectDirection[0];
        }
        else if (compassInfo.compass.state == Compass.Direction.Right)
        {
            tileInfo.edge.forward = edgeDirection[1];
            tileInfo.edge.backward = edgeDirection[0];
            tileInfo.edge.left = edgeDirection[3];
            tileInfo.edge.right = edgeDirection[2];

            tileInfo.wall.forward = connectDirection[1];
            tileInfo.wall.backward = connectDirection[0];
            tileInfo.wall.left = connectDirection[3];
            tileInfo.wall.right = connectDirection[2];
        }
        else if (compassInfo.compass.state == Compass.Direction.Backward)
        {
            tileInfo.edge.forward = edgeDirection[3];
            tileInfo.edge.backward = edgeDirection[2];
            tileInfo.edge.left = edgeDirection[0];
            tileInfo.edge.right = edgeDirection[1];

            tileInfo.wall.forward = connectDirection[3];
            tileInfo.wall.backward = connectDirection[2];
            tileInfo.wall.left = connectDirection[0];
            tileInfo.wall.right = connectDirection[1];
        }
        else if (compassInfo.compass.state == Compass.Direction.Left)
        {
            tileInfo.edge.forward = edgeDirection[0];
            tileInfo.edge.backward = edgeDirection[1];
            tileInfo.edge.left = edgeDirection[2];
            tileInfo.edge.right = edgeDirection[3];

            tileInfo.wall.forward = connectDirection[0];
            tileInfo.wall.backward = connectDirection[1];
            tileInfo.wall.left = connectDirection[2];
            tileInfo.wall.right = connectDirection[3];
        }

        tileInfo.IsEdgeCube = IsEdgeCube;
        tileInfo.IsConnectCube = IsConnectCube;
        tileInfo.OnTheWall = OnTheWall;
        tileInfo.DownWall = DownWall;
    }
}
