using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    const int MAPSIZE = 29;

    int[,] map;
    int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 }; //Next position x, y
    int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
    bool[,] visited = new bool[MAPSIZE, MAPSIZE]; 
    bool findEmpty = false; //Empty is Goal
    int[,] remove = new int[MAPSIZE, MAPSIZE]; //Keys to be removed

    public GameObject bossUI;
    public GameObject smokeUI; //Coffee UI
    UITimer timer;
    GameObject obstacle; 
    
    GameManager manager;
    StageGenerate generate;

    GameObject soundSource;
    ItemSound itemSound;

    public Material[] mat = new Material[3];

    Transform tf;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        generate = GameObject.Find("StageGenerator").GetComponent<StageGenerate>();

        soundSource = GameObject.Find("ItemSoundSource");
        if(soundSource != null)
            itemSound = soundSource.GetComponent<ItemSound>();

        timer = smokeUI.GetComponent<UITimer>();
        tf = gameObject.GetComponent<Transform>();
        map = generate.mapData; //Load mapData

    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Smoke") || other.CompareTag("Mail") || other.CompareTag("Bomb")) //Handling Obstacle Conflicts
        {
                ObstacleCollision(other);
        }

        if (other.CompareTag("Boss"))
        {
            manager.GameOver();
        }

        if (other.CompareTag("Key"))
        {
            int x, y; //Keyname split
            string keyName = other.name;
            keyName = keyName.Trim('(', ')');
            string[] parts = keyName.Split(',');
             x = int.Parse(parts[0]);
             y = int.Parse(parts[1]);


            GameObject.Find("(" + x + "," + y + ")").GetComponent<MeshRenderer>().materials = mat;  //Eating key

            if(map[x,y]==1)
            {
                if (soundSource != null)
                    itemSound.SoundPlay("Money");
                manager.keyCount++;
            }    
                
            map[x, y] = 9;
           

            edgeRule(x, y);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bomper")) 
        {
                ObstacleCollision(collision.gameObject.GetComponent<Collider>());
        }
    }
 
    public void ObstacleCollision(Collider obstacle) //Obstacle collision handling function
    {
        if (obstacle.CompareTag("Mail"))
        {
            itemSound.SoundPlay("Mail");
            if (GameObject.FindWithTag("Boss") ==  null)
            {
                Vector3 bossPosition = tf.position - tf.forward * 3;
                Instantiate(bossUI, bossPosition, Quaternion.identity);
                bossUI.SetActive(true);
            }
            else
            {
                manager.GameOver();
            }
        }
        else if (obstacle.CompareTag("Smoke"))
        {
            if (soundSource != null)
                itemSound.SoundPlay("Coffee");

            if (smokeUI.activeSelf) //Smoke is already active
                timer.uiTimer = 0.0f; //Timer Reset
            else
                smokeUI.SetActive(true);
             
        }
        else if (obstacle.CompareTag("Bomb"))
        {
            if (soundSource != null)
                itemSound.SoundPlay("Bomb");
            manager.GameOver();
        }
        else if(obstacle.CompareTag("Bomper"))
        {
            if (soundSource != null)
                itemSound.SoundPlay("PostIt");
            gameObject.GetComponent<Player>().ChangeMove();
        }

    }

    void edgeRule(int x, int y) //(DFS executed on a live key (starting point) in 8 directions (clockwise) from the location of the collision) Dead key = Wall, Obstacle or Empty = Arrival point
    {                           //(충돌한 위치에서 8방향(시계방향)에 있는 살아있는 열쇠(출발지점)에 DFS 실행) 죽은 열쇠 = 벽, 장애물이나 빈공간 = 도착지점 
        remove = new int[MAPSIZE, MAPSIZE]; //Remove array reset

        int i, xx, yy;

        for (i = 0; i < 8; i++)
        {
            xx = x + dx[i]; //x, y are colliding positions (충돌위치)
            yy = y + dy[i]; //dx, dy are next position parameters

            findEmpty = false;

            if (map[xx, yy] == 1) //Next position is key  
            {

                DFS(xx, yy);
                if (!findEmpty) //Fail to reach goal
                {
                    remove[xx, yy] = 1; //Mark position that executed DFS
                    RemoveKey();
                    Debug.Log("removed!");
                    break;
                }
            }
        }
    }

    void DFS(int x, int y)
    {
        int i, xx, yy;

        if (map[x, y] != 1)
            findEmpty = true; //Reach goal
        else
        {
            for (i = 0; i < 8; i++)
            {
                xx = x + dx[i];
                yy = y + dy[i];

                if (findEmpty)
                    continue;

                if (map[xx, yy] != 9 && visited[xx, yy] == false)
                {

                    visited[xx, yy] = true; //mark visited node 방문한 노드 표시
                    DFS(xx, yy);
                    visited[xx, yy] = false; 

                    if (!findEmpty && remove[xx, yy] == 0) //Mark the way one has passed 지나온 길을 전역변수에 할당
                        remove[xx, yy] = 1; 
                }
            }

        }
    }

    void RemoveKey()
    {
        for (int i = 0; i < MAPSIZE; i++)
        {
            for (int j = 0; j < MAPSIZE; j++)
            {
                if (remove[i, j] == 1)
                {
                    Debug.Log(i + ", " + j);
                    GameObject.Find("(" + i + "," + j + ")").GetComponent<MeshRenderer>().materials = mat; //Eating key
                    map[i, j] = 9;
                    manager.keyCount++;
                }
            }
        }
        if (soundSource != null)
            itemSound.SoundPlay("RuleMoney");
    }

}
