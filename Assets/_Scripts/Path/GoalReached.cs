using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSystem;

public class GoalReached : MonoBehaviour
{
    private bool touched = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!touched && other.CompareTag("Player"))
        {
            GameManager.instance.WinGame();
            if (RiverGenerator.instance != null)
            {
                RiverGenerator.instance.GenerateRealWorld();
            }
            if (DissolveTransition.instance != null)
            {
                DissolveTransition.instance.startDissolve(2f);
            }

            touched = true;
        }
        
    }
}
