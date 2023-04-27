using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    private int index;
    public Text IndexText;
    public Image minimap;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void OpenMap()
    {
        index = GameObject.FindObjectOfType<LoadGame>().Index;
        IndexText.text = "PREVIEW STAGE " + index;
        minimap.sprite = Resources.Load<Sprite>("Minimap/Minimap" + index); //Load Map
    }
}
