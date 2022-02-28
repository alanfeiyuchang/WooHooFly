using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelSelection : MonoBehaviour
{
    public int levelIndex;
    private bool reached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!reached)
        {
            MapTransition.instance.LevelTransition(levelIndex);
            reached = true;
        }
    }
}
