using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject target;
    public Transform targetP;  // �÷��̾��� ��ġ�� ������ ����

    public float distance = 2f;  // ī�޶�� �÷��̾� ���� �Ÿ�
    public float height = 1f;  // ����

    static public float smoothSpeed = 0.4f;  //�ε巯�� ����
    public static float changeSpeed = 0.1f; //ī�޶� �������� ����

    private Vector3 velocity = Vector3.zero;  // ī�޶� �̵� �� ����� �ӵ� ����

    public AudioSource bossAudioSource; //bossSound
    public AudioClip bossAudioClip;
    private float volume = 0.05f;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        targetP = target.GetComponent<Transform>();

        BossSoundPlay();

    }

    void LateUpdate()
    {
        // ī�޶��� ��ġ�� �ε巴�� �̵���Ű�� ���� Lerp �Լ� ���
        Vector3 targetPosition = targetP.position + Vector3.up * height - targetP.forward * distance;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothSpeed);

        // ī�޶� �÷��̾ �ٶ󺸵��� ȸ����Ŵ
        transform.LookAt(targetP);
    }

    static public void smoothSpeedUp()
    {
        smoothSpeed -= changeSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�÷��̾�� �浹");
        }
    }

    public void BossSoundPlay()
    {
        if (!MuteManager.EffectIsMuted)
        {
            bossAudioSource.clip = bossAudioClip;
            volume = 0.3f;

            bossAudioSource.loop = true;
            bossAudioSource.volume = volume;
            bossAudioSource.Play();
        }
    }
}
