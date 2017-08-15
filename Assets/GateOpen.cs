using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateOpen : MonoBehaviour
{
    // === Public Variables ====
    public Transform Left;
    public Transform Right;
    public float ChangeSpeed;

    public float ChangeTime;
    public float InRangeTime;

    // === Private Variables ====
    Transform player;

    bool open = false;
    bool close = false;

    float currentChangeTime;
    float currentinRangeTime;

    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").transform;
        InvokeRepeating("Detection", 0, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (open && Left.localPosition.x < 2.5)
        {
            currentChangeTime += Time.deltaTime;
            Left.localPosition = Left.localPosition - Vector3.left * ChangeSpeed;
            Right.localPosition = Right.localPosition - Vector3.right * ChangeSpeed;
        }
        if (currentChangeTime > ChangeTime && open)
        {
            currentChangeTime = 0;
            close = true;
            open = false;
        }
        if (close && Left.localPosition.x > 1)
        {
            currentChangeTime += Time.deltaTime;
            Left.localPosition = Left.localPosition + Vector3.left * ChangeSpeed;
            Right.localPosition = Right.localPosition + Vector3.right * ChangeSpeed;
        }
        if (currentChangeTime > ChangeTime && close)
        {
            currentChangeTime = 0;
            close = false;
            open = false;
            Left.localPosition = new Vector3(1, Left.localPosition.y, 2.5f);
            Right.localPosition = new Vector3(0, Right.localPosition.y, Right.localPosition.z);
        }

    }

    void Detection()
    {
        //Debug.Log("to door distance " + (player.position - transform.position).magnitude);
        if ((player.position - transform.position).magnitude < 8)
        {
            player.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
            player.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            currentinRangeTime++;
            if (currentinRangeTime > InRangeTime && !open)
            {
                open = true;
            }
        }
        bool dontclose = false;
        foreach (var gameobj in GameObject.FindGameObjectsWithTag("Droid"))
        {
            var trans = gameobj.transform;
            if ((trans.position - transform.position).magnitude < 10)
            {
                dontclose = true;
                if (!open)
                {
                    open = true;
                }
            }
        }

        if ((player.position - transform.position).magnitude > 10 && !close && !dontclose)
        {
            close = true;
            currentinRangeTime = 0;
        }
        if ((player.position - transform.position).magnitude > 15)
        {
            player.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
            player.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
