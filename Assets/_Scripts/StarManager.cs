using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int starNum = 0;
    private GameObject[,] star = new GameObject[3,2];
    void Awake()
    {
        star[0, 0] = transform.GetChild(0).gameObject;
        star[0, 1] = transform.GetChild(1).gameObject;
        star[1, 0] = transform.GetChild(2).gameObject;
        star[1, 1] = transform.GetChild(3).gameObject;
        star[2, 0] = transform.GetChild(4).gameObject;
        star[2, 1] = transform.GetChild(5).gameObject;

    }

    // Update is called once per frame
    /*void Update()
    {
        changeStar(starNum);
    }*/
    public void changeStar(int num)
    {
        for(int i = 0; i < 3; i++)
        {
            /*Debug.Log(star[i, 0]);
            Debug.Log(star[i, 1]);*/
            if (num > 0)
            {
                star[i, 0].SetActive(true);
                star[i, 1].SetActive(false);
                num--;
            }
            else
            {
                star[i, 1].SetActive(true);
                star[i, 0].SetActive(false);
            }
        }
    }
}
