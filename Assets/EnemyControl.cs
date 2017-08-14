using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    // === Public Variables ====
    public GameObject Laser;
    public Transform FirePoint;
    public float NormalSpeed;
    public float ChaseSpeed;
    public float FireSpeed;

    public enum AIState {PATROL, SEARCH, ENGAGE, IDLE, RETREAT};

    // === Private Variables ====
    Transform player;
    NavMeshAgent agent;
    Animator animator;
    AIState state = AIState.IDLE;
    float currentCooldown;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.PATROL:
                break;

            case AIState.SEARCH:
                break;

            case AIState.ENGAGE:
                break;

            case AIState.IDLE:
                break;

            case AIState.RETREAT:
                break;
        }
    }

    void FixedUpdate()
    {
        //Cooldown Counters
        currentCooldown = (currentCooldown - Time.fixedDeltaTime < 0) ? 0 : currentCooldown - Time.fixedDeltaTime;
    }

    void Engage()
    {
        agent.destination = player.position;
    }

    void Fire()
    {
        if (currentCooldown == 0)
        {

            Instantiate(Laser, FirePoint.position, Quaternion.identity);
        }
    }
}
