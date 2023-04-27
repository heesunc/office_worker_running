using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    //private const float DISTANCE = 5f;
    //private const float HEIGHT = 4f;

    //public Transform target;  // 플레이어의 위치를 저장할 변수
    //Vector3 targetPosition;
    //public Player player;

    //static public float smoothSpeed = 0.33f;  // 카메라 이동 시 부드러운 감속을 위한 변수
    //private Vector3 velocity = Vector3.zero;  // 카메라 이동 시 사용할 속도 벡터

    //private Transform tf; //gameObject.Transform
    //private int direction = 1;

    //void Start()
    //{
    //    tf = gameObject.GetComponent<Transform>();
    //}

    //void LateUpdate()
    //{
    //    direction = player.getMove();

    //    //카메라를 타깃 포지션으로 이동.
    //    if (direction == 1)
    //    {
    //        targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE;
    //    }
    //    else //direction == -1
    //    {
    //        targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE * 3f;
    //    }

    //    //카메라 위치와 방향 업데이트
    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
    //    transform.LookAt(target);
    //}

    //public void smoothSpeedUp()
    //{
    //    if (smoothSpeed > 0.1f)
    //    {
    //        smoothSpeed -= 0.1f;
    //    }
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("wall"))
    //    {
    //        Image image = collision.gameObject.GetComponent<Image>();

    //        Color color = image.color;
    //        color.a = 0f;
    //        image.color = color;

    //        //StartCoroutine(Fadein(image));
    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("wall"))
    //    {
    //        Image image = collision.gameObject.GetComponent<Image>();

    //        Color color = image.color;
    //        color.a = 0f;
    //        image.color = color;

    //        //StartCoroutine(Fadeout(image));
    //    }
    //}

    Renderer rend;
    Transform child;
    Material material;
    Color color;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("인식은 함");

        GameObject c = collider.gameObject;

        if (c.CompareTag("wall"))
        {
            int count = c.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                child = c.transform.GetChild(i);
                rend = child.gameObject.GetComponent<Renderer>();
                material = rend.material;
                color = material.color;
                material.shader = Shader.Find("Transparent/Diffuse");
                material.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Debug.Log("탈출");

        GameObject c = collider.gameObject;

        if (c.CompareTag("wall"))
        {
            int count = c.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                child = c.transform.GetChild(i);
                rend = child.gameObject.GetComponent<Renderer>();
                material = rend.material;
                material.shader = Shader.Find("Transparent/Diffuse");
                material.color = color;
            }
        }
    }
}
