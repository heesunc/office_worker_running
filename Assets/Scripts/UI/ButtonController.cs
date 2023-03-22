using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Option.getController() == Controller.SWIPE)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
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
