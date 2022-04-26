using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class HintTrigger : MonoBehaviour
{

    public UnityEvent HintEvent;

    private void OnTriggerEnter(Collider other) {
        StartCoroutine(wait(other));

        
    }

    IEnumerator wait(Collider other) {
        yield return new WaitForSeconds(0.1f);
        // Debug.Log("hit occur" + other.name);
        if (other.CompareTag("Player")) {
            HintEvent?.Invoke();
        }
    }
}
