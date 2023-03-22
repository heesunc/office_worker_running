using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float uiTimer;
    public float limitTime =10.0f;

    // Start is called before the first frame update
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {
        uiTimer += Time.deltaTime; //UI Å¸ÀÌ¸Ó
        if (uiTimer >= limitTime)
        {
            gameObject.SetActive(false);
            uiTimer = 0.0f;
        }

    }
}
