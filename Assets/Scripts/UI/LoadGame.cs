using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public string SceneToLoad;
    public int Index = 0;
    public int numberOfStage = 2;
 
    public GameObject stageSelection;
    private Text stageIndexText;


    public void StartGame()
    {
        SceneManager.LoadScene(SceneToLoad);
        DontDestroyOnLoad(transform.gameObject);

    }

    public void UpIndex()
    {
        if (Index < numberOfStage)
            Index++;
    }

    public void DownIndex()
    {
        if (Index > 0)
            Index--;
    }

    public void Start()
    {
        stageIndexText = stageSelection.GetComponentInChildren<Text>();
        
    }

    public void Update()
    {
        stageIndexText.text = Index + "/" + numberOfStage;
    }
}