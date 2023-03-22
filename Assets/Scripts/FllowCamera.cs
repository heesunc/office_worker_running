using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FllowCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 8, -15);
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(30, 0, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
        
    }
}
