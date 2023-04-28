using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSize : MonoBehaviour
{
    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep; // 게임 실행 중 화면이 꺼지지 않게
        Screen.SetResolution(1920,1080,true); // 사이즈 고정
    }
}
