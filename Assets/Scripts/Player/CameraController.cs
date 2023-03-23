using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float DISTANCE = 5f;
    private const float HEIGHT = 4f;

    public Transform target;  // 플레이어의 위치를 저장할 변수
    Vector3 targetPosition;
    public Player player;

    static public float smoothSpeed = 0.4f;  // 카메라 이동 시 부드러운 감속을 위한 변수
    private Vector3 velocity = Vector3.zero;  // 카메라 이동 시 사용할 속도 벡터

    private Transform tf; //gameObject.Transform
    private int direction = 1;

    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        direction = player.getMove();

        //카메라를 타깃 보지션으로 이동.
        if (direction == 1)
        {
            targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE;
        }
        else //direction == -1
        {
            targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE * 3f;
        }

        //카메라 위치와 방향 업데이트
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
        transform.LookAt(target);
    }
}
