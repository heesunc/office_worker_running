using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Controller
{
    SWIPE,
    BUTTON
}

public class Option : MonoBehaviour
{
    public GameObject option;

    static Controller controller = Controller.BUTTON;

    public static void ChangeToSwipe()
    {
        controller = Controller.SWIPE;
    }

    public static void ChangeToButton()
    {
        controller = Controller.BUTTON;
    }

    public static Controller getController()
    {
        return controller;
    }

    public void openOption()
    {
        option.SetActive(true);
    }

    public void closeOption()
    {
        option.SetActive(false);
    }
}
