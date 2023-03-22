using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    GameManager manager;

    public float limitTime;
    public Text textTimer;
    int min;
    float sec;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        limitTime -= Time.deltaTime;

        if (limitTime >= 60f)
        {
            min = (int)limitTime / 60;
            sec = limitTime % 60;
            textTimer.text = min + " : " + (int)sec;
        }
        else if (limitTime < 60f && limitTime > 0)
        {
            textTimer.text = "0 :" + (int)limitTime;       
        }
        else
        {
            textTimer.text = "시간 종료";
            manager.GameOver();
        }
    }
}
