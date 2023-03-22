using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public void OnClickRestartBtn()
{
    // 게임 오버가 되어 시간이 멈춘 상태에서, 시간을 다시 흐르게하고 현재 씬을 다시 보여준다
    Time.timeScale = 1;
	SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
}
