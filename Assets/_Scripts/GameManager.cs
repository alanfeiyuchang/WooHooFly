using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    //variables
    [SerializeField] private List<CubeCollider> Tiles;
    public GameState CurrentState = GameState.playing;
    public enum GameState
    {
        playing,
        paused,
        win
    };

    private void Start()
    {
        CurrentState = GameState.playing;
    }

    public void CheckWin()
    {
        bool win = true;
        foreach (CubeCollider tile in Tiles)
        {
            if (tile.SideColor == CubeCollider.color.red)
            {
                win = false;
                break;
            }
        }
        
        if (win)
        {
            CurrentState = GameState.win;
            Debug.Log("***************WON***************");
        }
        else
        {
            Debug.Log("Not yet");
        }
    }
}
