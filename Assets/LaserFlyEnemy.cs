using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFlyEnemy : MonoBehaviour
{
    // === Public Variables ====
    public float Speed;


    // === Private Variables ====
    Transform player;
    Vector3 targetDirection;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("PlayerCenter").transform;
        transform.LookAt(player);
        targetDirection = (player.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetDirection * Speed * Time.deltaTime;
        if ((transform.position - player.position).magnitude > 50)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Droid")
        {
            Destroy(gameObject);
        }
    }
}
