using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    GameManager manager;
    public Text GetMoneyText;
    public Text RestMoneyText;
    public Text GetMoney;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // 획득한 돈
        GetMoneyText.text="획득한 돈 : "+manager.keyCount+"$";

        // 남은 돈 개수
        int rest = manager.keyFind.Length;
        int restmoney = rest-manager.keyCount;
        RestMoneyText.text="남은 돈 : "+restmoney;

        // 획득한 돈 개수
        GetMoney.text="획득한 돈 갯수 : "+manager.keyCount;
    }
}