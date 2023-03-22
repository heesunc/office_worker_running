using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseUI;
    private Text PauseSec;

    public void ExitBtn()
    {
        // Pause를 누르고 "나가기" 버튼을 누르면 시작화면으로 이동
        SceneManager.LoadScene("StartScene");
        Time.timeScale = 1f;
    }

    public void ProgressBtn()
    {
        StartCoroutine(GetEnumerator());
    }

    public IEnumerator GetEnumerator()
    {
        PauseSec = PauseUI.GetComponentInChildren<Text>();
        PauseSec.enabled = true;
        for (int i = 3; i >= 1; i--)
        {
            PauseSec.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }

        PauseUI.SetActive(false);
        Time.timeScale = 1f;
        Debug.Log("3초 뒤 다시 게임 시작");        
    }
}
