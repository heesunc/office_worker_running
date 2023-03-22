using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Revive : MonoBehaviour
{
    public GameObject GameOver_UI;
    public GameObject SecUI;
    GameManager manager;
    public Button ReviveBtn;
    public GameObject player;

    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ReviveBtn.onClick.AddListener(OnReviveButtonClick);
    }

    public void OnReviveButtonClick()
    {
        RespawnPlayer(player);

        GameOver_UI.SetActive(false);
        SecUI.SetActive(true);
        Sec secScript = SecUI.GetComponent<Sec>();
        secScript.StartSecond(); // 3초 세는 코루틴 함수 실행
        manager.isOver = false;
        ReviveBtn.interactable = false;
        Score.Rescore(); // 부활하고 먹은 점수 반영되도록
    }

    // 구현하고 싶었다.. 안 된다.. ㅠㅠ
    public Vector3 ChooseRespawnPosition()
    {
        if (player == null || player.transform == null)
        {
            Debug.LogError("Invalid player object");
            return Vector3.zero;
        }

        float playerX = Mathf.FloorToInt(player.transform.position.x);
        float playerZ = Mathf.FloorToInt(player.transform.position.z);
        float offset = Mathf.FloorToInt(player.transform.position.y);
        Vector3 center = new Vector3(playerX, offset, playerZ);

        //float distanceBetween = 7f;

        Vector3 respawnPosition = Vector3.zero;
        Vector3 firstPosition = new Vector3(-7f, 0f, 112f);
        int maxAttempts = 10;
        int attempt = 0;

        while (attempt < maxAttempts)
        {
            //int randomX = UnityEngine.Random.Range(-1, 2);
            //int randomZ = UnityEngine.Random.Range(-1, 2);
            //Vector3 position = center * distanceBetween;
            Collider[] colliders = Physics.OverlapBox(center, new Vector3(7f, 0.1f, 7f) / 2, Quaternion.identity);            
            if (colliders.Length == 0)
            {
                respawnPosition = center;
                break;
            }
            attempt++;
        }

        // if (attempt >= maxAttempts)
        // {
        //     Debug.LogError("Failed to find a valid respawn position after " + maxAttempts + " attempts");
        //     respawnPosition = firstPosition;
        //     return respawnPosition;
        // }
        return respawnPosition;
    }

    public void RespawnPlayer(GameObject player)
    {
        Vector3 respawnPosition = ChooseRespawnPosition();
        player.transform.position = respawnPosition;
    }
}