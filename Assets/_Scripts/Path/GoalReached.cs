using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TileSystem;

public class GoalReached : MonoBehaviour
{
    private bool touched = false;
    public GameObject finalMap;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("touched: " + touched + " collider: " + other.tag + " win: "+ GameManager.instance.CheckWin());
        if (!touched && other.CompareTag("Player") && GameManager.instance.CheckWin())
        {
            GameManager.instance.WinGame();
            if (finalMap != null) {
                finalMap.GetComponent<FinalTransition>().Begin();
            }
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
