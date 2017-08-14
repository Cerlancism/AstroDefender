using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    // === Public Variables ====
    public Transform FirePoint;
    public GameObject Laser;

    // === Private Variables ====
    NavMeshAgent agent;
    Transform navArrow;
    MeshRenderer navArrowRen;
    MeshRenderer navArrowRen2;
    Animator animator;

    bool fired = false;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navArrow = GameObject.Find("NavArrow").transform;
        navArrowRen = navArrow.GetChild(0).GetComponent<MeshRenderer>();
        navArrowRen2 = navArrow.GetChild(1).GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.remainingDistance < 0.1f && agent.remainingDistance != 0 && animator.GetBool("Run"))
        {
            fired = false;
            animator.SetBool("Run", false);
        }
        if (Input.GetButtonUp("Run"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                fired = false;
                agent.updatePosition = true;
                agent.destination = hit.point;
                navArrowRen.material.color = Color.green;
                navArrowRen2.material.color = Color.green;
                navArrow.position = hit.point;
                animator.SetBool("Run", true);
            }
        }
        if (Input.GetButtonUp("Fire1") && Time.timeScale != 0)
        {
            animator.SetTrigger("Shoot");
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                agent.destination = transform.position - (transform.position - hit.point).normalized * 0.1f;
                transform.position = transform.position + (transform.position - hit.point).normalized * 0.15f;
                navArrowRen.material.color = Color.red;
                navArrowRen2.material.color = Color.red;
                navArrow.position = hit.point;
                fired = true;
                Fire();
            }
        }
        if (fired)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(navArrow.position - transform.position), 0.1f);
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            
        }
    }

    void Fire()
    {
        Instantiate(Laser, FirePoint.position, Quaternion.identity);
    }
}
