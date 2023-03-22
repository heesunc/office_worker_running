using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public string SceneToLoad;
    
    public void StartGame()
    {
        SceneManager.LoadScene(SceneToLoad);
    }
}