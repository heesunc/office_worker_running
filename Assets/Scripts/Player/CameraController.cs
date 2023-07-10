using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    Renderer rend;
    Transform child;
    Material material;
    Color color;

    //for Rotate
    Transform tf;
    Transform parent;
    float t = 0;
    bool motion = false;

    //폭쥭폭쥭
    public GameObject party;

    //카메라 거리 유지
    public Transform player;
    public Transform camera;
    private Vector3 dist;

    void Start()
    {
        tf = GetComponent<Transform>();
        parent = player;

        //dist = player.position - camera.position;
    }

    void Update()
    {
        //tf.position = player.position - dist;
        //tf.LookAt(player.position);

        if (motion == true)
        {
            t += Time.deltaTime;
            tf.RotateAround(parent.position, parent.up, 180 * Time.deltaTime);
            if (t > 1f)
            {
                Vector3 p = parent.position + parent.right * 5;
                Instantiate(party, parent.position + parent.up * 4f + parent.right * 4, Quaternion.identity);
                Instantiate(party, parent.position + parent.up * 4f - parent.right * 4, Quaternion.identity);
                motion = false;
            }
        }
    }

    public void motionStart()
    {
        motion = true;
    }

    private void OnTriggerEnter(Collider collider)
    {
        GameObject c = collider.gameObject;

        if (c.CompareTag("wall"))
        {
                rend = c.GetComponent<Renderer>();
                material = rend.material;
                color = material.color;
                material.shader = Shader.Find("Transparent/Diffuse");
                material.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        GameObject c = collider.gameObject;

        if (c.CompareTag("wall"))
        {
            //int count = c.transform.childCount;
            //for (int i = 0; i < count; i++)
            //{
            //    child = c.transform.GetChild(i);
                rend = c.GetComponent<Renderer>();
                material = rend.material;
                material.shader = Shader.Find("Transparent/Diffuse");
                material.color = color;
            //}
        }
    }
}
