using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using WooHooFly.NodeSystem;

[ExecuteInEditMode]
public class MouseRotation : MonoBehaviour
{
    //public Transform Compass;
    private float rotateAngle = 90;
    private Vector3 rotationAngle;
    private Vector3 targetAngle;
    private float TurnDuration = 1f;
    [SerializeField] private Transform Axis;

    //public bool enableMouseRotation;
    //public RotationLink[] rotationLinks;
    public RotationLinker[] rotationLinksTransit;
    public RotationLinker[] rotationLinksCorner;
    public UnityEvent<float> rotationEvent;

    public static MouseRotation instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        targetAngle = transform.eulerAngles;
        //UpdateOrientation(targetAngle.y);
        UpdateLinkers(Mathf.RoundToInt(transform.eulerAngles.y));
    }

    private void OnEnable()
    {
        //UpdateOrientation(targetAngle.y);
        UpdateLinkers(Mathf.RoundToInt(transform.eulerAngles.y));
    }

    void Update()
    {
        //if (!enableMouseRotation)
        //    return;
        if (!Application.isPlaying)
        {
            UpdateLinkerParents();
            return;
        }

        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        rotationAngle = this.transform.up * rotateAngle;

    }

    // How the whole level is rotated related to world axis
    //private void UpdateOrientation(float angle)
    //{
    //    if (GameManager.instance == null)
    //        return;
    //    if (angle > 0 && angle <= 90)
    //    {
    //        GameManager.instance.levelDirection = Direction.Right;
    //    }
    //    else if (angle > 90 && angle <= 180)
    //    {
    //        GameManager.instance.levelDirection = Direction.Backward;
    //    }
    //    else if (angle > 180 && angle <= 270)
    //    {
    //        GameManager.instance.levelDirection = Direction.Left;
    //    }
    //    else if (angle > 270 && angle < 360 || angle == 0)
    //    {
    //        GameManager.instance.levelDirection = Direction.Forward;
    //    }
    //}

    /// <summary>
    /// Enable/Disable linker when rotate to certain angle
    /// </summary>
    /// <param name="angle"></param>
    //private void UpdateLinkersOld (float angle)
    //{
    //    foreach(RotationLink link in rotationLinks)
    //    {
    //        if(link.checkNodeATransform == null && link.checkNodeBTransform == null)
    //        {
    //            if (angle == link.activeAngle)
    //            {
    //                EnableLink(link.nodeA, link.nodeB, true);
    //            }
    //            else
    //            {
    //                EnableLink(link.nodeA, link.nodeB, false);
    //            }
    //        }
    //        else
    //        {
    //            if (link.checkNodeATransform != null)
    //            {
    //                if(link.checkNodeATransform.transform.localPosition == link.transform)
    //                {
    //                    if (angle == link.activeAngle)
    //                    {
    //                        EnableLink(link.nodeA, link.nodeB, true);
    //                    }
    //                    else
    //                    {
    //                        EnableLink(link.nodeA, link.nodeB, false);
    //                    }
    //                }   
    //            }
    //            else if (link.checkNodeBTransform != null)
    //            {
    //                if (link.checkNodeBTransform.transform.localPosition == link.transform)
    //                {
    //                    if (angle == link.activeAngle)
    //                    {
    //                        EnableLink(link.nodeA, link.nodeB, true);
    //                    }
    //                    else
    //                    {
    //                        EnableLink(link.nodeA, link.nodeB, false);
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    private void EnableLink(Node nodeA, Node nodeB, bool state)
    {
        if (nodeA == null || nodeB == null)
            return;
        if (TutorialManager.current != null) {
            if (state) {
                TutorialManager.current.HighlightPath();
            }
        }
        
        nodeA.EnableTransitEdge(nodeB, state);
        nodeB.EnableTransitEdge(nodeA, state);
    }

    private void UpdateLinkers(float angle)
    {

        foreach (RotationLinker link in rotationLinksTransit)
        {
            if (angle == link.activeAngle)
            {
                EnableTransitLink(link.nodeA, link.nodeB, true);
            }
            else
            {
                EnableTransitLink(link.nodeA, link.nodeB, false);
            }
        }

        foreach (RotationLinker link in rotationLinksCorner)
        {
            if (angle == link.activeAngle)
            {
                EnableCornerLink(link.nodeA, link.nodeB, true);
            }
            else
            {
                EnableCornerLink(link.nodeA, link.nodeB, false);
            }
        }
    }

    private void EnableTransitLink(Node nodeA, Node nodeB, bool state)
    {
        if (nodeA == null || nodeB == null)
            return;

        nodeA.EnableTransitEdge(nodeB, state);
        nodeB.EnableTransitEdge(nodeA, state);
        // if (state)
        //     MapTransition.instance?.GetCurrentCubeControllerScript().FindAccessibleNode();
    }

    private void EnableCornerLink(Node nodeA, Node nodeB, bool state)
    {
        if (nodeA == null || nodeB == null)
            return;

        nodeA.EnableCornerEdge(nodeB, state);
        nodeB.EnableCornerEdge(nodeA, state);
    }

    private void UpdateLinkerParents()
    {
        foreach (RotationLinker link in rotationLinksTransit)
        {
            if (link.nodeA == null)
                continue;
            link.NodeA_Annotation = GetAnnotation(link.nodeA);
            if (link.nodeB == null)
                continue;
            link.NodeB_Annotation = GetAnnotation(link.nodeB);
        }

        foreach (RotationLinker link in rotationLinksCorner)
        {
            if (link.nodeA == null)
                continue;
            link.NodeA_Annotation = GetAnnotation(link.nodeA);
            if (link.nodeB == null)
                continue;
            link.NodeB_Annotation = GetAnnotation(link.nodeB);
        }
    }

    private string GetAnnotation(Node node)
    {
        return node.transform.parent.parent.name + "_" + node.transform.parent.name;
    }

    private Vector3 clampAngle(Vector3 target)
    {
        float x;
        if (target.x > 180)
            x = target.x - 360;
        else if (target.x < -180)
            x = target.x + 360;
        else
            x = target.x;

        float y;
        if (target.y > 180)
            y = target.y - 360;
        else if (target.y < -180)
            y = target.y + 360;
        else
            y = target.y;

        float z;
        if (target.z > 180)
            z = target.z - 360;
        else if (target.z < -180)
            z = target.z + 360;
        else
            z = target.z;
        return new Vector3(x, y, z);
    }

    public void RotateMapCW()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        StartCoroutine(RotateMapAnim(rotateAngle));
    }

    public void RotateMapCCW()
    {
        if (GameManager.instance.CurrentState != GameManager.GameState.playing)
            return;
        StartCoroutine(RotateMapAnim(-1 * rotateAngle));
    }


    IEnumerator RotateMapAnim(float angle)
    {
        Debug.Log(angle);
        GameManager.instance.CurrentState = GameManager.GameState.rotating;
        TutorialManager.current?.DeactiveArrow();
        float t = 0;
        Vector3 startAngleY = transform.eulerAngles;
        Vector3 endAngle = transform.eulerAngles;
        endAngle[1] += angle;
        endAngle = clampAngle(endAngle);
        while (t < TurnDuration)
        {
            float turnAngle = (Time.deltaTime / TurnDuration) * angle;
            t += Time.deltaTime;
            //float rotationAngle = Mathf.Lerp(0, angle, t / TurnDuration);
            transform.RotateAround(Axis.position, Vector3.up, turnAngle);
            yield return null;
        }
        transform.eulerAngles = endAngle;
        GameManager.instance.CurrentState = GameManager.GameState.playing;

        //UpdateOrientation(Mathf.RoundToInt(transform.eulerAngles.y));
        //UpdateLinkersOld(Mathf.RoundToInt(transform.eulerAngles.y));
        UpdateLinkers(Mathf.RoundToInt(transform.eulerAngles.y));

        if (rotationEvent != null)
            rotationEvent.Invoke(angle); // need to tell rotation direction
    }

}
