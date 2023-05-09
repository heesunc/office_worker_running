using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBtn : MonoBehaviour
{
    public GameObject ExitUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitUI.SetActive(true);
        }
    }

    public void StayBtn()
    {
        ExitUI.SetActive(false);
        // Stay 버튼 누르면 ExitUI창 꺼짐
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
