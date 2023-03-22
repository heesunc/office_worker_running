using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update

    int[,] map = { { 0, 0, 0, 0, 0, 0},
                   { 0, 9, 9, 9, 1, 0 },
                   { 0, 9, 1, 9, 1, 0 },
                   { 0, 9, 9, 9, 1, 0 },
                   { 0, 9, 9, 9, 1, 0 },
                   { 0, 0, 0, 0, 0, 0}};

    int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };
    int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
    bool[,] visited = new bool[6, 6];
    bool findEmpty = false;
    int[,] remove = new int[6, 6];
    
 
    void Start()
    {
        
        test(1,3);
        
       

     
    }

    // Update is called once per frame
    void Update()
    {
       
    }


   void DFS(int x, int y)
    {
        int i, xx, yy;

        if (map[x, y] != 1)
            findEmpty = true;
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
                    
                    visited[xx, yy] = true;
                    DFS(xx, yy);
                    visited[xx, yy] = false;


                    if (!findEmpty && remove[xx, yy] == 0)
                        remove[xx, yy] = 1;

                }
                

            }
            
        }
    }

    void test(int x, int y)
    {
        remove = new int[29, 29];

        int i, xx, yy;
        
        for (i=0; i<8; i++)
        {
            xx = x + dx[i];
            yy = y + dy[i];

            Debug.Log(i);
           
            findEmpty = false;

            if (map[xx, yy] == 1)
            {
                
                DFS(xx, yy);
                if(!findEmpty)
                {
                    remove[xx, yy] = 1;
                    RemoveKey();
                    Debug.Log("Removed!!!");
                    break;
                }

            }

        }

        Debug.Log("end");
    }

  void RemoveKey()
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (remove[i, j] == 1)
                {
                    Debug.Log(i + ", " + j);
                    //GameObject.Find("(" + i + "," + j + ")").SetActive(false);
                    map[i, j] = 9;


                }
            }
        }
    }
}
