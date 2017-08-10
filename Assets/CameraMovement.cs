using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // === Public Variables ====


    // === Private Variables ====


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-1f * Time.deltaTime, 0, -1f * Time.deltaTime, Space.World);
    }
}
