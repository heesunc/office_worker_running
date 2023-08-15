using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    static public int time = 1000;

    static public void timeC()
    {
        if (time == 1000)
            time = 400;
        else
            time = 1000;
    }

    static public int tween = 2;

    static public void tweenC()
    {
        if (tween < 3)
            tween++;
        else 
            tween = 0;
    }

    static public bool rotate = true;

    static public void rotateC()
    {
        rotate = !rotate;
    }


    public Text textTime;
    public Text textTween;
    public Text textRotate;

    void Update()
    {
        textTime.text = "time: " + time.ToString();
        textTween.text = "tween: " + tween.ToString();
        textRotate.text = "rotate: " + rotate.ToString();
    }

}
