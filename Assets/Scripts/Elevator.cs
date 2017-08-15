using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Elevator : MonoBehaviour
{
    // === Public Variables ====
    public float MoveSpeed;

    // === Private Variables ====
    Transform player;
    NavMeshAgent agent;
    bool moveUp = true;
    bool isMoving;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = player.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (moveUp)
            {
                transform.position = transform.position + Vector3.up * MoveSpeed * Time.deltaTime;
                player.position = player.position + Vector3.up * MoveSpeed * Time.deltaTime;
                player.rotation = Quaternion.identity;
                if (transform.localPosition.z >= 22)
                {
                    isMoving = false;
                    moveUp = false;
                    player.SetParent(null);
                    agent.enabled = true;
                    player.position = player.position + Vector3.forward * 3;
                    player.GetComponent<Collider>().enabled = true;
                }
            }
            else
            {
                transform.position = transform.position - Vector3.up * MoveSpeed * Time.deltaTime;
                player.position = player.position - Vector3.up * MoveSpeed * Time.deltaTime;
                player.rotation = Quaternion.identity;
                if (transform.localPosition.z <= 0)
                {
                    isMoving = false;
                    moveUp = true;
                    player.SetParent(null);
                    agent.enabled = true;
                    player.GetComponent<Collider>().enabled = true;
                    player.position = player.position - Vector3.forward * 3;
                }
            }

        }
    }

    public void Move()
    {
        isMoving = true;
    }
}
