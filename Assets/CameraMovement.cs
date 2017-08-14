using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // === Public Variables ====


    // === Private Variables ====
    Transform playerTransform;
    Vector3 cameraFollow;
    //Camera Reset Holder
    Transform cameraLerp;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        cameraFollow = transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, playerTransform.position + cameraFollow, 0.05f);

        if ((Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f) &&
            (Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 30 > 10 && Camera.main.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * 30 < 70))
        {
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 30;
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            Camera.main.fieldOfView = 30;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {

        }
    }
}
