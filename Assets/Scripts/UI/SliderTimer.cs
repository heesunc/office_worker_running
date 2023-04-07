using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderTimer : MonoBehaviour
{
    GameManager manager;
    public Button reviveBtn;

    Slider slTimer;
    public float fSliderTime = 1000.0f;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        slTimer = GetComponent<Slider>();
        slTimer.maxValue = fSliderTime;
        slTimer.value = fSliderTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (slTimer.value > 0.0f)
        {
            slTimer.value -= Time.deltaTime;
        }
        else
        {
            Debug.Log("Time Out");
            manager.GameOver();
            reviveBtn.interactable = false;
        }
    }
}
