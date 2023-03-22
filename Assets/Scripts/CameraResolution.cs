using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    //모바일 해상도 설정
    // Start is called before the first frame update
    void Awake()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheigt = ((float)Screen.width / Screen.height) / ((float)16 / 9); //(가로 / 세로)
        float scalewidth = 1f / scaleheigt;
        if (scaleheigt < 1)
        {
            rect.height = scaleheigt;
            rect.y = (1f - scaleheigt) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
