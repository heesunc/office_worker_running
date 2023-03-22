using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;  // 플레이어의 위치를 저장할 변수

    public float distance = 10f;  // 카메라와 플레이어 간의 거리
    public float height = 5f;  // 카메라와 플레이어 간의 높이
    public float smoothSpeed = 0.5f;  // 카메라 이동 시 부드러운 감속을 위한 변수

    private Vector3 velocity = Vector3.zero;  // 카메라 이동 시 사용할 속도 벡터

    void LateUpdate()
    {
        // 카메라의 위치를 부드럽게 이동시키기 위해 Lerp 함수 사용
        Vector3 targetPosition = target.position + Vector3.up * height - target.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // 카메라가 플레이어를 바라보도록 회전시킴
        transform.LookAt(target);
    }
}
