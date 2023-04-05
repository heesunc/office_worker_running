using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadGame : MonoBehaviour
{
    public string SceneToLoad;
    public int Index;
    public int numberOfStage = 2;
 
    public GameObject stageSelection;
    private Text stageIndexText;


    public void StartGame()
    {
        LoadingSceneController.LoadScene(SceneToLoad);
        //SceneManager.LoadScene(SceneToLoad);
        DontDestroyOnLoad(transform.gameObject);

    }

    public void UpIndex()
    {
        if (Index < numberOfStage && Index <= PlayerPrefs.GetInt("clearData"))
            Index++;
    }

    public void DownIndex()
    {
        if (Index > 1)
            Index--;
    }

    public void Awake()
    {
        stageIndexText = stageSelection.GetComponentInChildren<Text>();
        if (PlayerPrefs.GetInt("curIndex") == 0)
            PlayerPrefs.SetInt("curIndex", 1);

        Index = PlayerPrefs.GetInt("curIndex");
        
    }

    public void Update()
    {
        stageIndexText.text = Index + "/" + numberOfStage;
    }
}