using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class StageGenerate : MonoBehaviour
{
    const int MAPSIZE = 29;
    private int timeLimit;

    public GameObject moneyPrefab; //Items Prefabs
    public GameObject fileStackPrefab;
    public GameObject mailPrefab;
    public GameObject postItPrefab;
    public GameObject coffeePrefab;

    public GameObject moneyParent; //Items Parent
    public GameObject fileStackParent;
    public GameObject mailParent;
    public GameObject postItParent;
    public GameObject coffeeParent;

    public GameObject tutorial;

    public float distance = 7.0f;
    public static int stageIndex; //Stage selection
    public int testIndex;
    public TextAsset stageFile;
    public int[,] mapData;
    
    private GameObject stageManager;
    private Transform player;
    private Transform playerTarget;
    private SliderTimer timer;
    AdmobRewardAd admobRewardAd;

    private void Awake()
    {
        stageManager = GameObject.Find("StageManager");
        player = GameObject.FindWithTag("Player").transform;
        playerTarget = GameObject.Find("PlayerTarget").transform;
        timer = GameObject.Find("SliderTimer").GetComponent<SliderTimer>();

        if (stageManager != null)
        {
            stageIndex = stageManager.GetComponent<LoadGame>().Index; //Stage Selection
        }
        else
        {
            stageIndex = testIndex;
        }  


        TextAsset stageData = Resources.Load<TextAsset>("Stage" + stageIndex); //Load Stage.text
        PlayerPrefs.SetInt("curIndex", stageIndex); 
        string[] lines = stageData.text.Split('\n');

        timeLimit = int.Parse(lines[29]); //Last line is TimeLimit
        mapData = new int[MAPSIZE, MAPSIZE]; 

        for (int i = 0; i < MAPSIZE; i++) //Split stage data
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                int val = int.Parse(lines[i][j].ToString());
                mapData[i, j] = val;
            }
        }

        timer.fSliderTime = timeLimit; //set timer
      
        for (int i = 0; i < MAPSIZE; i++) //Create Stage
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                if (mapData[i, j] == 1) //Create money
                {
                    GameObject money = Instantiate(moneyPrefab, new Vector3(i * distance, 1.6f, j * distance), Quaternion.identity);
                    money.name = "(" + i + "," + j + ")";
                    money.transform.parent = moneyParent.transform;

                }
                else if (mapData[i, j] == 2) //Create fileStack
                {
                    GameObject fileStack = Instantiate(fileStackPrefab, new Vector3(i * distance, 1.5f, j * distance), Quaternion.Euler(-90.0f, 0, 0));
                    fileStack.name = "(" + i + "," + j + ")";
                    fileStack.transform.parent = fileStackParent.transform;
                }
                else if (mapData[i, j] == 3) //Create mail
                {
                    GameObject mail = Instantiate(mailPrefab, new Vector3(i * distance, 1.5f, j * distance), Quaternion.Euler(0, 90.0f, 0));
                    mail.name = "(" + i + "," + j + ")";
                    mail.transform.parent = mailParent.transform;
                }
                else if (mapData[i, j] == 4) //Create postIt
                {
                    GameObject postIt = Instantiate(postItPrefab, new Vector3(i * distance, 1.5f, j * distance), Quaternion.Euler(0, 90.0f, 0));
                    postIt.name = "(" + i + "," + j + ")";
                    postIt.transform.parent = postItParent.transform;
                }
                else if (mapData[i, j] == 5) //Create Coffee
                {
                    GameObject coffee = Instantiate(coffeePrefab, new Vector3(i * distance, 0.7f, j * distance), Quaternion.Euler(-90.0f, 0, 0));
                    coffee.name = "(" + i + "," + j + ")";
                    coffee.transform.parent = coffeeParent.transform;
                }
                else if(mapData[i, j] == 6) //move playerPos
                {
                    Vector3 playerPos = new Vector3(i * distance, 0.0f, j * distance);
                    player.position = playerPos;
                }
                else if(mapData[i, j] == 7) //set player direction
                {
                    playerTarget.position = new Vector3(i * distance, 0.0f, j * distance);
                }
            }
        }
        player.LookAt(playerTarget);

        if(stageIndex == 1)
        {
            OpenTutorial();
        }
    }

    public void NextStageLoad()
    {
        if (admobRewardAd != null)
        {
            admobRewardAd.ShowAd(() =>
            {
                Time.timeScale = 1.0f;
                stageManager.GetComponent<LoadGame>().Index++;
                LoadingSceneController.LoadScene("Stage");
            });
        }
        else
        {
            Debug.LogError("계속하기 버튼 누를 시 광고 오류");
        }
    }

    void OpenTutorial()
    {
        Time.timeScale = 0.0f;
        tutorial.SetActive(true);
    }

    public void CloseTutorial()
    {
        Time.timeScale = 1.0f;
        tutorial.SetActive(false);

    }
    void Start()
    {
       admobRewardAd = FindObjectOfType<AdmobRewardAd>();
    }

    void update()
    {

    }
}