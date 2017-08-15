using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // === Public Variables ====
    public GameObject Droid;
    public int WayPointRegion;
    public int MaxDefaultCoount;
    public static int droidCount;
    public static int droidsKilled;
    // === Private Variables ====


    // Use this for initialization
    void Start()
    {
        InvokeRepeating("SpawnDroid", 0, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (droidsKilled > 100)
        {
            foreach (var gameobj in GameObject.FindGameObjectsWithTag("Droid"))
            {
                gameobj.GetComponent<EnemyControl>().state = EnemyControl.AIState.RETREAT;
            }
            CancelInvoke("SpawnDroid");
        }
    }

    void SpawnDroid()
    {
        if (droidCount < MaxDefaultCoount)
        {
            int degree = Random.Range(0, 360);
            float radian = degree * Mathf.Deg2Rad;
            float x = Mathf.Cos(radian);
            float z = Mathf.Sin(radian);
            Vector3 spawnPoint = new Vector3(x, 0, z) * 10;
            spawnPoint = transform.position + spawnPoint;
            GameObject droid = Instantiate(Droid, spawnPoint, Quaternion.identity);
            droid.GetComponent<EnemyControl>().SpawnPoint = transform;
            droid.GetComponent<EnemyControl>().WayPointGroup = WayPointRegion;
            droid.GetComponent<EnemyControl>().state = EnemyControl.AIState.PATROL;
            droidCount++;
        }
    }
}
