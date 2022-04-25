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
        if(CreditUI.activeInHierarchy == false)
        {
            panel.SetActive(true);
        }
     }*/
    public GameObject CreditUI;
    public GameObject panel;
    public void playGame()
    {
        SceneManager.LoadScene("Prototype");
    }
    public void Credit()
    {
        //SceneManager.LoadScene("Credit");
        CreditUI.SetActive(true);
        panel.SetActive(false);
    }
    public void CreditClose()
    {
        panel.SetActive(true);
    }
    public void quit()
    {
        Application.Quit();
    }
}
