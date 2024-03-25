using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllClearExit : MonoBehaviour
{
    public Image credit;
    public PauseBtn pause;

    public void Start()
    {
        credit.gameObject.SetActive(false);
    }

    public void Exit()
    {
        if (credit.gameObject.activeSelf)
            pause.ExitBtn();
        else
            credit.gameObject.SetActive(true);
    }
}
