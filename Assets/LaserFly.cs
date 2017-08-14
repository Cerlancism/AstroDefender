using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFly : MonoBehaviour
{
    // === Public Variables ====
    public float Speed;

    // === Private Variables ====
    Transform firePoint;
    Transform player;
    Transform navArrow;
    Vector3 targetDirection;

    // Use this for initialization
    void Start()
    {
        firePoint = GameObject.Find("FirePoint").transform;
        navArrow = GameObject.Find("NavArrow").transform;
        player = GameObject.Find("PlayerCenter").transform;
        if ((player.position - navArrow.position).magnitude > 10f)
        {
            Vector3 relativeTarget = new Vector3(navArrow.position.x, navArrow.position.y + 1.85f, navArrow.position.z);
            transform.LookAt(new Vector3(player.position.x, firePoint.position.y, player.position.z));
            float targetYNormal = (relativeTarget - (new Vector3(player.position.x, player.position.y, player.position.z))).normalized.y;
            targetDirection = (firePoint.position - (new Vector3(player.position.x, player.position.y, player.position.z))).normalized;
            targetDirection = new Vector3(targetDirection.x, targetYNormal, targetDirection.z);
        }
        else
        {
            transform.LookAt(new Vector3(player.position.x, firePoint.position.y, player.position.z));
            targetDirection = (firePoint.position - (new Vector3(player.position.x, player.position.y, player.position.z))).normalized;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += targetDirection * Speed * Time.deltaTime;
        if ((transform.position - firePoint.position).magnitude > 50)
        {
            Destroy(gameObject);
        }
    }
}
