using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //GameObject[] keyFind; //Scene에 존재하는 키의 수
    public GameObject[] keyFind; //Scene에 존재하는 키의 수

    public int keyCount; //획득한 키의 수
    public GameObject keyCountUI;
    private TextMeshProUGUI keyCountText;

    public bool isClear;
    public bool isOver;
    public GameObject GameOver_UI;
    public GameObject GameClear_UI;
    private GameObject boss_UI;
    public GameObject smoke_UI;

    public Animator anim;

    GameObject soundSource;
    PlayerSound playerSound;

    private Player player;
    private Fade fade;


    void Start()
    {
        keyCount = 0; //player가 획득한 key 개수 0으로 초기화
        keyFind = GameObject.FindGameObjectsWithTag("Key"); //Scene 전체의 키 찾기
        keyCountText = keyCountUI.GetComponentInChildren<TextMeshProUGUI>(); //keyCountUI의 자식 keyCountText의 Text 컴포넌트 get
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();


        soundSource = GameObject.Find("PlayerSoundSource");
        if (soundSource != null)
            playerSound = soundSource.GetComponent<PlayerSound>();
            
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        fade = GameObject.Find("Fade").GetComponent<Fade>();
    }

    void Update()
    {
        
        keyCountText.text = (keyFind.Length - keyCount).ToString(); //KeyCount UI

        if (keyCount >= keyFind.Length)
        {
            keyCount = keyFind.Length;
            GameClear();
        }
    }    

    public void GameOver()
    {
        if (isOver || isClear)
        {
            return;
        }
        isOver = true;
        Debug.Log("GameOver!");

        playerSound.SoundPlay("GameOver");
        
        player.speed = 0.1f;
        anim.SetBool("Dead", true);
        Invoke("GameOverTest", 3f);
    }

    private void GameOverTest()
    {

        Time.timeScale = 0;
        GameOver_UI.SetActive(true);
        InactiveUI(); //이미 활성화 된 UI들 제거
    }

    public void GameClear()
    {
        if (isClear || isOver)
        {
            return;
        }
        isClear = true;

        if(PlayerPrefs.GetInt("curIndex") > PlayerPrefs.GetInt("clearData"))
        {
            PlayerPrefs.SetInt("clearData", PlayerPrefs.GetInt("curIndex"));
        }

        playerSound.SoundPlay("GameClear");
        Debug.Log("GameClear!");
        player.speed = 0.1f;
        //fade.B_Fadeout();
        anim.SetBool("Clear", true);
        Invoke("GameClearTest", 2.5f);

        //Time.timeScale = 0;
        //GameClear_UI.SetActive(true);
        //InactiveUI();
    }

    // fade위한 것이였던 함수...
    private void GameClearTest()
    {
        Time.timeScale = 0;
        fade.B_Fadeover();
        GameClear_UI.SetActive(true);
        InactiveUI();
    }

    private void InactiveUI()
    {
        smoke_UI.SetActive(false);
        boss_UI = GameObject.FindWithTag("Boss");
        if (boss_UI!= null)
        {
            Destroy(boss_UI);
        }
    }

}
