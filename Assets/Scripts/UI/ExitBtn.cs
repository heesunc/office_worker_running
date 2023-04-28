using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtn : MonoBehaviour
{
    GameObject ExitUI;
    // Start is called before the first frame update
    void Start()
    {
        ExitUI = GameObject.Find("Exit");
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitUI.SetActive(true);
            }
        }
    }

    public void stayBtn()
    {
        ExitUI.SetActive(false); // Exit UI 비활성화
    }

    public void quitBtn()
    {
        Application.Quit(); // 앱 종료
    }
}
