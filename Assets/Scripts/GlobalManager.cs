using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    // === Public Variables ====
    public static int Score;
    public AudioMixer mixer;
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

    public void LoadPlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetMusic(float volume)
    {
        mixer.SetFloat("AudioMusic", volume);
    }

    public void SetSFX(float volume)
    {
        mixer.SetFloat("AudioSFX", volume);
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
