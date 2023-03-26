using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    //애니메이션은 Entry에 바로 적용해서 보여주면 될 것 같아요.
    //만약 스크롤바 대신 원이 돌아가는 로딩을 원해도 애니메이션 Entry로 넣으면 될 것 같습니다.
    //Animation and Loading circle can be implemented by applying it to entry. 

    static string NextScene;
    public Scrollbar scb;
    private float timer;
  
    public static void LoadScene(string SceneName)
    {
        SceneManager.LoadScene("LoadingScene");
        NextScene = SceneName;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(NextScene);
        op.allowSceneActivation = false;

        timer = 0f;

        while(!op.isDone)
        {
            yield return null;

            Debug.Log("로딩 중"); //디버그용
            scb.size = op.progress;

            if(op.progress >= 0.9f)
            {
                timer += Time.unscaledDeltaTime;
                Debug.Log(timer); //디버그용

                if(timer >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}