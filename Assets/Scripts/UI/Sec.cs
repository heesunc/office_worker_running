using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sec : MonoBehaviour
{
    public Text PauseSecT;
    public GameObject SecUI;

    public void StartSecond()
    {
        StartCoroutine(GetEnumerator());
    }

    public IEnumerator GetEnumerator()
    {
        for (int i = 3; i >= 1; i--)
        {
            PauseSecT.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        SecUI.SetActive(false); // 3초 UI 끄기
        Time.timeScale = 1f;
        Debug.Log("3초 뒤 다시 게임 시작");        
    }
}
