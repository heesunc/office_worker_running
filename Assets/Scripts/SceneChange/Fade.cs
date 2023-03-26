using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public GameObject image;
    private Image img;

    Color color;

    private void Awake()
    {
        img = image.GetComponent<Image>();
        color = img.color;
        color.a = 0f;

        //image.SetActive(true);
    }

    //void Start()
    //{
    //    StartCoroutine(Fadein());
    //}

    //���� true�� ����.
    //IEnumerator Fadein() //ȭ���� ������ ��. 
    //{
    //    while (color.a > 0)
    //    {
    //        color.a -= Time.deltaTime;
    //        image.color = color;
    //
    //        yield return null;
    //    }
    //
    //    fadeimage.SetActive(false);
    //}

    IEnumerator Fadeout(string Name)// bool Loading) 
    {
        while (color.a < 1)
        {
            color.a += Time.deltaTime;
            img.color = color;

            yield return null;
        }

        if (color.a >= 1)
        {
            //if (Loading)
            //LoadingSceneController.LoadScene(Name); //�ε����� �̿��� �ε�.
            //else
            SceneManager.LoadScene(Name);
            yield break;
        }
    }

    public void B_Fadeout(string Name)
    {
        image.SetActive(true);
        StartCoroutine(Fadeout(Name)); //, false));
    }

    //public void L_Fadeout(string Name)
    //{
    //    fadeimage.SetActive(true);
    //    StartCoroutine(Fadeout(Name, true));
    //}
}
