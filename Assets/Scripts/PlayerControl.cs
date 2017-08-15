using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerControl : MonoBehaviour
{
    // === Public Variables ====
    public Transform FirePoint;
    public GameObject Laser;
    public int MaxHP;
    public int CurrentHP;
    public int FirstAidHP;
    public int MaxAmmo;
    public int CurrentAmmo;
    public int ClipSize;

    // === Private Variables ====
    NavMeshAgent agent;
    Transform navArrow;
    MeshRenderer navArrowRen;
    MeshRenderer navArrowRen2;
    Animator animator;

    bool fired = false;
    int CurrentAmmoClip;

    bool isInLift = false;

    // Use this for initialization
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        navArrow = GameObject.Find("NavArrow").transform;
        navArrowRen = navArrow.GetChild(0).GetComponent<MeshRenderer>();
        navArrowRen2 = navArrow.GetChild(1).GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();

        CurrentHP = MaxHP;
        CurrentAmmo = MaxAmmo;
        CurrentAmmoClip = ClipSize;

        GlobalManager.UpdateHP(CurrentHP);
        GlobalManager.UpdateAmmo(CurrentAmmoClip, CurrentAmmo);
        GlobalManager.UpdateScore(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.enabled)
        {
            return;
        }
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
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                navArrowRen.material.color = Color.red;
                navArrowRen2.material.color = Color.red;
                navArrow.position = hit.point;
                fired = true;
                Fire(hit);
            }
        }
        if (fired)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(navArrow.position - transform.position), 0.1f);
        }

        if (Input.GetKeyUp(KeyCode.P) || Input.GetKeyUp(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
            GlobalManager.TogglePause();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            Reload();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "EnemyLaser")
        {
            Debug.Log("HP: " + CurrentHP);
            CurrentHP = CurrentHP - 1 <= 0 ? 0 : CurrentHP - 1;
            GlobalManager.UpdateHP(CurrentHP);
            if (CurrentHP == 0)
            {
                Die();
            }
        }

        if (tag == "Health")
        {
            Debug.Log("HP gained->: " + CurrentHP);
            CurrentHP = CurrentHP + FirstAidHP >= 100 ? 100 : CurrentHP + FirstAidHP;
            GlobalManager.UpdateScore(10);
            GlobalManager.UpdateHP(CurrentHP);
            Destroy(other.gameObject);
        }

        if (tag == "Ammo")
        {
            Debug.Log("Current Ammo: " + CurrentAmmo);
            CurrentAmmo = CurrentAmmo * 2 + ClipSize >= MaxAmmo ? MaxAmmo : CurrentAmmo * 2 + ClipSize;
            GlobalManager.UpdateAmmo(CurrentAmmoClip, CurrentAmmo);
            GlobalManager.UpdateScore(10);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Elevator" && !isInLift)
        {
            agent.enabled = false;
            animator.SetBool("Run", false);
            gameObject.GetComponent<Collider>().enabled = false;
            //transform.SetParent(collision.gameObject.transform);
            transform.position = other.gameObject.transform.position;
            other.gameObject.transform.parent.GetComponent<Elevator>().Move();
        }
    }

    void Reload()
    {
        if (CurrentAmmo == 0)
        {
            Debug.Log("No Ammo");
        }
        int toFill = ClipSize - CurrentAmmoClip;
        toFill = CurrentAmmo - toFill >=  0 ? toFill : CurrentAmmo;
        CurrentAmmo -= toFill;
        CurrentAmmoClip += toFill;
        Debug.Log("Clip: " + CurrentAmmoClip + " AmmoLeft: " + CurrentAmmo);
        GlobalManager.UpdateAmmo(CurrentAmmoClip, CurrentAmmo);
    }

    void Fire(RaycastHit hit)
    {
        if (CurrentAmmoClip > 0)
        {
            animator.SetTrigger("Shoot");
            agent.destination = transform.position - (transform.position - hit.point).normalized * 0.1f;
            transform.position = transform.position + (transform.position - hit.point).normalized * 0.15f;
            Instantiate(Laser, FirePoint.position, Quaternion.identity);
            CurrentAmmoClip--;
            GlobalManager.UpdateAmmo(CurrentAmmoClip, CurrentAmmo);
        }
        else
        {
            Debug.Log("No Clip");
        }
    }

    void Die()
    {
        GetComponent<Collider>().enabled = false;
        GameObject.Find("MESH_Infantry").GetComponent<SkinnedMeshRenderer>().enabled = false;
        GlobalManager.ShowDeath();
        InvokeRepeating("DieAnimation", 0, 1/60f);
    }

    void DieAnimation()
    {

    }
}
