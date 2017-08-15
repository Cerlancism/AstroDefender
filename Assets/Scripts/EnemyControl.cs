using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    // === Public Variables ====
    public GameObject Laser;
    public GameObject AmmoDrop;
    public GameObject Healthdrop;
    public Transform FirePoint;
    public Transform SpawnPoint;
    public Transform[] WayPointArray;
    public AIState state = AIState.IDLE;
    public float NormalSpeed;
    public float ChaseSpeed;
    public float FireSpeed;
    public float CloseEngage;
    public int WayPointGroup;
    public int HitPoints;

    public enum AIState {PATROL, SEARCH, ENGAGE, IDLE, RETREAT};

    public static float outOfRangeTime = 0;

    public AudioClip ExplodeSFX;

    // === Private Variables ====
    Transform player;
    NavMeshAgent agent;
    Animator animator;
    Vector3 searchPoint;
    int searchCount = 0;
    int currentWayPoint = 0;
    int currentHitPoints;
    float currentCooldown;
    float time;
    bool isRetreating;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = NormalSpeed;
        animator = transform.GetChild(0).GetComponent<Animator>();
        currentHitPoints = HitPoints;
        var GO = GameObject.FindGameObjectsWithTag("WayPointGroup");
        WayPointArray = new Transform[GO.Length];
        for (int i = 0; i < WayPointArray.Length; i++)
        {
            foreach (var gobj in GO)
            {
                int index = int.Parse(gobj.name.Replace("WP ", ""));
                if (index == i)
                {
                    WayPointArray[i] = gobj.transform;
                }
            }
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case AIState.PATROL:
                Patrol();
                break;

            case AIState.SEARCH:
                Search();
                break;

            case AIState.ENGAGE:
                Engage();
                break;

            case AIState.IDLE:
                Idle();
                break;

            case AIState.RETREAT:
                ReTreat();
                break;
        }
    }

    void FixedUpdate()
    {
        //Cooldown Counters
        currentCooldown = (currentCooldown - Time.fixedDeltaTime < 0) ? 0 : currentCooldown - Time.fixedDeltaTime;
        time = (time + Time.fixedDeltaTime)> 2 ? 0 : time + Time.fixedDeltaTime;
    }

    //Hit points laser hit
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerLaser")
        {
            currentHitPoints--;
            if (currentHitPoints == 0)
            {
                EnemySpawner.droidCount--;
                EnemySpawner.droidsKilled++;
                SpawnDrops();
                GlobalManager.UpdateScore(10);
                Destroy(gameObject);
            }

            //Alert Other droids in region to chase if one is hit
            foreach (var gameobj in GameObject.FindGameObjectsWithTag("Droid"))
            {
                if (gameobj.GetComponent<EnemyControl>().WayPointGroup == WayPointGroup)
                {
                    gameobj.GetComponent<EnemyControl>().state = AIState.SEARCH;
                }
            }
        }
    }

    void SpawnDrops()
    {
        int random = Random.Range(1, 101);
        if (random > 66)
        {
            int r = Random.Range(0, 2);
            if (r == 1)
            {
                Instantiate(AmmoDrop, transform.position + Vector3.up, Quaternion.identity);
            }
            else
            {
                Instantiate(Healthdrop, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }

    void Patrol()
    {
        if (time>1)
        {
            time = 0;
            if (currentWayPoint == 0)
            {
                agent.destination = WayPointArray[WayPointGroup].GetChild(0).position;
            }
            
        }
        if (agent.remainingDistance < 1)
        {
            currentWayPoint = currentWayPoint + 1 == WayPointArray[WayPointGroup].childCount ? 0 : currentWayPoint + 1;
            agent.destination = WayPointArray[WayPointGroup].GetChild(currentWayPoint).position;
        }
        if ((player.position - transform.position).magnitude < 15)
        {
            outOfRangeTime = 0;
            state = AIState.ENGAGE;
        }
    }

    void Search()
    {
        if (searchCount == 0)
        {
            searchPoint = new Vector3(player.position.x + Random.Range(-20, 20), player.position.y, player.position.z + Random.Range(-20, 20));
            agent.destination = searchPoint;
            searchCount++;
        }
        if (searchCount > 0 && agent.remainingDistance < 1)
        {
            searchPoint = new Vector3(searchPoint.x + Random.Range(-40, 40), searchPoint.y , searchPoint.z + Random.Range(-40, 40));
            agent.destination = searchPoint;
            searchCount++;
        }
        if ((player.position - transform.position).magnitude < 30)
        {
            searchCount = 0;
            outOfRangeTime = 0;
            state = AIState.ENGAGE;
        }
        if (searchCount == 5)
        {
            searchCount = 0;
            state = AIState.IDLE;
        }
    }

    void Idle()
    {
        animator.SetTrigger("Idle");
        if (time>1)
        {
            agent.destination = transform.position;
            Invoke("IdleBreak", Random.Range(3, 5)); ;
            time = 0;
        }
        if ((player.position - transform.position).magnitude < 15)
        {
            outOfRangeTime = 0;
            state = AIState.ENGAGE;
        }

    }

    void IdleBreak()
    {
        state = AIState.PATROL;
    }

    void Engage()
    {
        if (outOfRangeTime > 5)
        {
            state = AIState.SEARCH;
        }
        if (time > 1) //update nav mesh agent every 1 sec
        {
            //Debug.Log("out of range time " + outOfRangeTime);
            time = 0;
            if ((transform.position - player.position).magnitude < CloseEngage)
            {
                agent.destination = transform.position;
            }
            else
            {
                agent.speed = ChaseSpeed;
                agent.destination = player.position;
            }
            if (outOfRangeTime > 5)
            {
                state = AIState.SEARCH;
            }
        }
        Fire();
    }

    void Fire()
    {
        if (currentCooldown == 0)
        {
            if ((player.position - transform.position).magnitude > CloseEngage * 2)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.GetChild(0).transform.position, player.position - transform.GetChild(0).transform.position, out hit, CloseEngage * 4))
                {
                    if (hit.collider.gameObject.tag != "Player" == true)
                    {
                        return;
                    }
                    Shoot();
                }
                else if ((player.position - transform.position).magnitude < CloseEngage * 4)
                {
                    Shoot();
                }
            }
            else if ((player.position - transform.position).magnitude < CloseEngage * 4)
            {
                Shoot();
            }
            
        }
    }

    void Shoot()
    {
        outOfRangeTime = 0;
        animator.SetTrigger("Shoot");
        transform.LookAt(player);
        Instantiate(Laser, FirePoint.position, Quaternion.identity);
        currentCooldown = FireSpeed;
    }

    void ReTreat()
    {
        if (!isRetreating)
        {
            isRetreating = true;
            agent.destination = SpawnPoint.position;
        }

        if ((transform.position - SpawnPoint.position).magnitude < 10)
        {
            Destroy(gameObject);
            EnemySpawner.droidCount--;
        }
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.GetChild(0).transform.position, transform.GetChild(0).transform.position + (player.position - transform.GetChild(0).transform.position).normalized * 40);
    //}
}
