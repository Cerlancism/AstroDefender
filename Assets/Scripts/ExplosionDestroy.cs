using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDestroy : MonoBehaviour
{
    // === Public Variables ====


    // === Private Variables ====
    float TTL = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TTL += Time.deltaTime;
        if (TTL > 5)
        {
            Destroy(gameObject);
        }

    }
}
