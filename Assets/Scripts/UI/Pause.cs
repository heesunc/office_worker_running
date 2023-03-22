using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public Button pauseBtn;
    public GameObject PauseUI;
    GameManager manager;
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    void Update()
    {
        //Debug.Log("Update");
        transform.Translate(Time.deltaTime, 0, 0);
    }
    private void FixedUpdate()
    {
        //Debug.Log("FixedUpdate");
    }

    public void OnTogglePauseButton()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; //멈추기
            PauseUI.SetActive(true);
        }
    }
}
