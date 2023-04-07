using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float DISTANCE = 5f;
    private const float HEIGHT = 4f;

    public Transform target;  // �÷��̾��� ��ġ�� ������ ����
    Vector3 targetPosition;
    public Player player;

    static public float smoothSpeed = 0.33f;  // ī�޶� �̵� �� �ε巯�� ������ ���� ����
    private Vector3 velocity = Vector3.zero;  // ī�޶� �̵� �� ����� �ӵ� ����

    private Transform tf; //gameObject.Transform
    private int direction = 1;

    void Start()
    {
        tf = gameObject.GetComponent<Transform>();
    }

    void LateUpdate()
    {
        direction = player.getMove();

        //ī�޶� Ÿ�� ���������� �̵�.
        if (direction == 1)
        {
            targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE;
        }
        else //direction == -1
        {
            targetPosition = target.position + Vector3.up * HEIGHT - target.forward * DISTANCE * 3f;
        }

        //ī�޶� ��ġ�� ���� ������Ʈ
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);
        transform.LookAt(target);
    }

    public void smoothSpeedUp()
    {
        if (smoothSpeed > 0.1f)
        {
            smoothSpeed -= 0.1f;
        }
    }
}
