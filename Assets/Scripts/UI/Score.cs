using UnityEngine;
using UnityEngine.UI;
using System;

public class Score : MonoBehaviour
{
    GameManager manager;
    public Text GetMoneyText;
    public Text RestMoneyText;
    public Text GetMoney;

    public static Action Rescore;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        UpdateMoneyTexts();
    }

    void Awake()
    {
        Rescore = () => { OnReviveButtonClick(manager.keyCount); };
    }
    void UpdateMoneyTexts()
    {
        // 획득한 돈
        GetMoneyText.text="획득한 돈 : "+manager.keyCount+"$";

        // 남은 돈 개수
        int rest = manager.keyFind.Length;
        int restmoney = rest-manager.keyCount;
        RestMoneyText.text="남은 돈 : "+restmoney;

        // 획득한 돈 개수
        GetMoney.text="획득한 돈 갯수 : "+manager.keyCount;
    }

    public void OnReviveButtonClick(int keyCount)
    {
        // keyCount 값을 업데이트함
        manager.keyCount = keyCount;

        // Text를 업데이트함
        UpdateMoneyTexts();
    }
}