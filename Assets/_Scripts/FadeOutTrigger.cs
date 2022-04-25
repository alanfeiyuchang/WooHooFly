using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutTrigger : MonoBehaviour
{
    [SerializeField] private PlayerAutoRotate par;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        par.FadeOut();
    }
}
