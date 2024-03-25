using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSize : MonoBehaviour
{
    private Camera cam;

    private int setWidth = 1920;
    private int setHeight = 1080;

    private int deviceWidth = Screen.width;
    private int deviceHeight = Screen.height;

    void Start()
    {
        cam = GetComponent<Camera>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 게임 실행 중 화면이 꺼지지 않게
        setAspect();
    }

    public void setAspect()
    {
        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if ((float)setWidth/setHeight < (float)deviceWidth/deviceHeight)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            cam.rect = new Rect((1f - newWidth) / 2f, 0, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight);
            cam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
}
