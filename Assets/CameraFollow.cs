
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] [Range(0.01f, 3f)]
    private float smoothSpeed = 0.125f;

    [SerializeField] private Vector3 offset;

    private Vector3 lastPosition;

    private Vector3 velocity = Vector3.zero;

    public static CameraFollow instance;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        target = MapTransition.instance.GetCurrentPlayerTransform();
        //offset = transform.position - target.position;
        offset = new Vector3(18, 18, -18);
    }

    // Update is called once per frame
    void Update()
    {
        // fix the camera when the game is not playing
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
        {
            transform.position = lastPosition;
        }
        else
        {
            Vector3 desiredPosition = target.position + offset;
            //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            //transform.position = smoothedPosition;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            lastPosition = transform.position;
        }
        
    }
    
    public void SetCameraTarget(Transform target)
    {
        this.target = target;
    }
}
