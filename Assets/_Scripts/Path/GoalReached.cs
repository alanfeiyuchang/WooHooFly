using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalReached : MonoBehaviour
{
    private bool touched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!touched && other.CompareTag("Player"))
        {
            GameManager.instance.WinGame();
            touched = true;
        }
        
    }
}
