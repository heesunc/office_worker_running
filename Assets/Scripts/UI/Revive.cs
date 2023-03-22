using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Revive : MonoBehaviour
{
    public GameObject GameOver_UI;
    GameManager manager;
    private Text ReviveSec;
    public Button ReviveBtn;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ReviveBtn.onClick.AddListener(OnReviveButtonClick);
    }

    public void OnReviveButtonClick()
    {
        StartCoroutine(GetEnumerator());
        ReviveBtn.interactable=false;
    }

    public IEnumerator GetEnumerator()
    {
        // 부활 후 3초 뒤 게임 계속할 수 있도록
        ReviveSec = GameOver_UI.GetComponentInChildren<Text>();
        ReviveSec.enabled = true;
        for (int i = 3; i >= 1; i--)
        {
            ReviveSec.text = i.ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        // 게임 다시 시작할 수 있도록
        GameOver_UI.SetActive(false);
        manager.isOver=false;
        Time.timeScale = 1f;

        // 부활하고 먹은 점수 반영되도록
        Score.Rescore();
    }
}