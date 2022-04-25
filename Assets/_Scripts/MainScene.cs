using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
   /* // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
   public void playGame()
    {
        SceneManager.LoadScene("Prototype");
    }
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void quit()
    {
        Application.Quit();
    }
}
