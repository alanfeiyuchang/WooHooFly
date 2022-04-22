using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class HintTrigger : MonoBehaviour
{

    public UnityEvent HintEvent;

    private void OnTriggerEnter(Collider other) {
        // Debug.Log("hit occur" + other.name);
        if (other.CompareTag("Player")) {
            // if (TutorialManager.current != null) {
            //     TutorialManager.current.TextHit();
            // }
            HintEvent?.Invoke();
        }
        
    }
}
