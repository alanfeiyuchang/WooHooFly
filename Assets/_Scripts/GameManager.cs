using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WooHooFly.NodeSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        instance = this;
    }

    //variables
    private List<GameObject> ChangableTiles = new List<GameObject>();
    public GameState CurrentState = GameState.playing;
    [HideInInspector]
    public Direction levelDirection = Direction.None;
    public enum GameState
    {
        restart,
        playing,
        paused,
        rotating,
        win
    };
    private bool levelComplete = false;

    private void Start()
    {
        CurrentState = GameState.playing;

        foreach (GameObject Obj in GameObject.FindGameObjectsWithTag("MapCube"))
        {
            if (Obj.name == "Changable_Tile")
            {
                ChangableTiles.Add(Obj);
            }
        }
    }


    public void CheckWin()
    {
        if (levelComplete || CurrentState == GameState.restart)
            return;

        bool win = true;
        foreach (var tile in ChangableTiles)
        {
            if (tile.GetComponent<MapColorChange>().MapColor == CubeCollider.color.red)
            {
                win = false;
                break;
            }
        }

        if (win)
        {
            CurrentState = GameState.win;
            Debug.Log("***************WON***************");
            UIController.instance.WinUI();
            levelComplete = true;
        }
        else
        {
            //Debug.Log("Not yet");
        }
    }
}
