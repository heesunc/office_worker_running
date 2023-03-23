using UnityEngine;
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
        //RespawnPlayer(player); // 플레이어 부활

        GameOver_UI.SetActive(false);
        SecUI.SetActive(true);
        Sec secScript = SecUI.GetComponent<Sec>();
        secScript.StartSecond(); // 3초 세는 코루틴 함수 실행
        manager.isOver = false;
        ReviveBtn.interactable = false;
        Score.Rescore(); // 부활하고 먹은 점수 반영되도록
    }

    // 안전한 곳으로 부활
    // 리스폰할 위치를 선택하는 함수
    public Vector3 ChooseRespawnPosition()
    {
        // 플레이어 위치에서 3x3 배열 추출
        float playerX = Mathf.FloorToInt(player.transform.position.x);
        float playerZ = Mathf.FloorToInt(player.transform.position.z);
        Vector3 center = new Vector3(playerX, 0f, playerZ);
        Vector3[,] positions = new Vector3[3, 3];
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 position = center + new Vector3(i, 0f, j);
                positions[i + 1, j + 1] = position;
            }
        }

        // 중앙 위치에 대해 3x3 영역에 있는 위치 중에서 콜라이더와 충돌하지 않는 위치 선택
        Vector3 respawnPosition = Vector3.zero; // 부활 위치 저장
        Collider[] colliders; // OverlapSphere 함수 사용하여 충돌 판정을 검사하기 위한 배열
        int maxAttempts = 10; // 부활 위치를 무작위로 선택하는 최대 시도 횟수
        int attempt = 0; // 현재 시도한 횟수를 저장하는 변수
        while (attempt < maxAttempts) // 부활 위치 선택
        {
            // 랜덤하게 배열 내 좌표 선택
            int randomX = UnityEngine.Random.Range(0, positions.GetLength(0)); // 0부터 2까지 무작위 정수를 선택하여 할당
            int randomZ = UnityEngine.Random.Range(0, positions.GetLength(1)); // 0부터 2까지 무작위 정수를 선택하여 할당

            // x 좌표값이 7의 배수인 위치를 찾음
            respawnPosition = positions[randomX, randomZ];
            if (Mathf.FloorToInt(respawnPosition.x) % 7 == 0)
            {
                // 선택한 위치가 중앙 위치와 같으면 continue
                if (randomX == 1 && randomZ == 1)
                {
                    continue;
                }

                // 선택한 위치가 콜라이더와 충돌하지 않으면 리스폰 위치로 선택
                // respawnPosition 위치에서 반지름만큼 크기의 충돌 판정을 검사하여 colliders 배열에 할당
                colliders = Physics.OverlapBox(center, new Vector3(7f, 0.1f, 7f) / 2, Quaternion.identity);
                if (colliders.Length == 0) // 부딪히는 콜라이더가 없다면 반복문 종료
                {
                    break;
                }
            }

            attempt++; // 그렇지 않은 경우 다시 새로운 위치 선택
        }

        // 리스폰 위치 반환
        Debug.Log(respawnPosition);
        return respawnPosition;
    }


    public void RespawnPlayer(GameObject player)
    {
        Vector3 respawnPosition = ChooseRespawnPosition();
        player.transform.position = respawnPosition;
    }
}