using UnityEngine;
using UnityEngine.UI;
using System;
public class Revive : MonoBehaviour
{
    public GameObject GameOver_UI;
    public GameObject SecUI;
    GameManager manager;
    public Animator anim;
    private Player player;
    public Button ReviveBtn;
    private GameObject cube;
    const int MAPSIZE = 29;
    const float SQUARESIZE = 7.0f;
    AdmobRewardAd admobRewardAd;

    private int[,] mapData;

    void Start()
    {
        cube = GameObject.Find("Revive");

        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        ReviveBtn.onClick.AddListener(OnReviveButtonClick);
        admobRewardAd = FindObjectOfType<AdmobRewardAd>();

        anim = GameObject.FindGameObjectWithTag("PlayerPoint").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("PlayerPoint").GetComponent<Player>();

        mapData = GameObject.Find("StageGenerator").GetComponent<StageGenerate>().mapData;
    }

    public void OnReviveButtonClick()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 cubePos;

        /*if (admobRewardAd != null)
        {
            admobRewardAd.ShowAd(() =>
            { */
                if (player.transform.rotation.y == 90 || player.transform.rotation.y == 270)
                {
                    // Player is facing along the X-axis
                    cubePos = new Vector3(playerPos.x - 7.0f, playerPos.y, playerPos.z);
                    cube.transform.position = cubePos;
                    if (Physics.Raycast(cubePos, Vector3.left, out RaycastHit hitInfo, 7.0f))
                    {
                        // There is a collider within 7.0 units of the position, set the cube position to -14.0f
                        cubePos = new Vector3(playerPos.x - 14.0f, playerPos.y, playerPos.z);
                        cube.transform.position = cubePos;
                        Debug.Log("뭔가가 있어 더 이동");
                    }
                    Debug.Log("X축 부활");
                }
                if (player.transform.rotation.y == 0 || player.transform.rotation.y == 180)
                {
                    // Player is facing along the Z-axis
                    cubePos = new Vector3(playerPos.x, playerPos.y, playerPos.z - 7.0f);
                    cube.transform.position = cubePos;
                    if (Physics.Raycast(cubePos, Vector3.back, out RaycastHit hitInfo, 7.0f))
                    {
                        // There is a collider within 7.0 units of the position, set the cube position to -14.0f
                        cubePos = new Vector3(playerPos.x, playerPos.y, playerPos.z - 14.0f);
                        cube.transform.position = cubePos;
                        Debug.Log("뭔가가 있어 더 이동");
                    }
                    Debug.Log("Z축 부활");
                }
                else
                {
                    Debug.Log("리스폰 오류");
                }

                //cube.transform.position = cubePos;
                player.transform.position = cube.transform.position;
                Debug.Log("로테이션 각도 : " +player.transform.rotation.y);
                Debug.Log("Rotation in degrees: " + transform.rotation.eulerAngles.y * Mathf.Rad2Deg);
                Debug.Log("부활 위치 확인 : " + cube.transform.position);

                // 맵 데이터 불러와서 하는 방법
                // playerDeathPos = player.transform.position;
                // int x = Mathf.RoundToInt(playerDeathPos.x / SQUARESIZE) + MAPSIZE / 2;
                // int z = Mathf.RoundToInt(playerDeathPos.z / SQUARESIZE) + MAPSIZE / 2;
                // x = Mathf.Clamp(x, 0, MAPSIZE - 1);
                // z = Mathf.Clamp(z, 0, MAPSIZE - 1);

                // if (mapData[x, z] == 0 || mapData[x, z] == 1) // 해당 좌표가 0 또는 1인 경우에만 부활 가능
                // {
                //     Vector3 respawnPos = new Vector3((x - MAPSIZE / 2) * SQUARESIZE, 1f, (z - MAPSIZE / 2) * SQUARESIZE);
                //     player.transform.position = respawnPos;
                //     Debug.Log(player.transform.position);
                // }
                // else
                // {
                //     Debug.Log("부활 불가능한 위치입니다.");
                // }

                anim.SetBool("Dead", false);
                player.reRun();
                GameOver_UI.SetActive(false);

                SecUI.SetActive(true);
                Sec secScript = SecUI.GetComponent<Sec>();
                secScript.StartSecond(); // 3초 세는 코루틴 함수 실행   

                manager.isOver = false;
                ReviveBtn.interactable = false;
                //Score.Rescore(); // 부활하고 먹은 점수 반영되도록
            /*});
        }
        else
        {
            Debug.LogError("부활 시 광고 오류");
        } */
    }
}