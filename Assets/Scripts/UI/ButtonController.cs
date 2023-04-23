using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject swipeBtn;
    // Start is called before the first frame update
    void Start()
    {
        if (Option.getController() == Controller.SWIPE)
        {
            swipeBtn.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            swipeBtn.SetActive(false);
        }
    }

    // Update is called once per frame
    public void onButton()
    {
        gameObject.SetActive(true);
    }

    public void offButton()
    {
        gameObject.SetActive(false);
    }
}
