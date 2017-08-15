using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalManager : MonoBehaviour
{
    // === Public Variables ====
    public static int Score;

    // === Private Variables ====


    // Use this for initialization
    void Start()
    {
        Score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        EnemyControl.outOfRangeTime += Time.fixedDeltaTime;
    }

    public static void UpdateHP(int HP)
    {
        GameObject.Find("HPText").GetComponent<Text>().text = "HP: " + HP;
    }

    public static void UpdateAmmo(int Clip, int Total)
    {
        GameObject.Find("AmmoText").GetComponent<Text>().text = "Ammo: " + Clip + " / " + Total;
    }

    public static void UpdateScore(int Value)
    {
        Score += Value;
        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score: " + Score;
    }

    public static void ShowDeath()
    {
        Time.timeScale = 0;
        GameObject.Find("DeathCanvas").GetComponent<Canvas>().enabled = true;
    }

    public static void ShowWin()
    {
        Time.timeScale = 0;
        GameObject.Find("WinCanvas").GetComponent<Canvas>().enabled = true;
    }

    public static void TogglePause()
    {
        GameObject.Find("PauseCanvas").GetComponent<Canvas>().enabled = !GameObject.Find("PauseCanvas").GetComponent<Canvas>().enabled;
    }
}
