using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (TutorialManager.current != null) {
                TutorialManager.current.ArrowHit();
            }
        }
        
    }
}
