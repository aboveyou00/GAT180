using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public MapController map;
    public WaveController waves;
    public ShopController shop;

    public Scene loseScene;
    public bool hasLost = false;

    public float playSpeed = 1;

    public int enemiesKilled = 0;
    public int enemiesBanished = 0;
    public int moneysSpent = 0;
    public float timeSurvived = 0;

    private void Start()
    {
        map.game = this;
        waves.game = this;
        shop.game = this;

        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (!hasLost)
        {
            timeSurvived += Time.deltaTime;
            if (checkForGameOver())
            {
                hasLost = true;
                Time.timeScale = 1;
                SceneManager.LoadScene(loseScene);
            }
        }
    }
    private bool checkForGameOver()
    {
        return shop.money < 0;
    }

    private void OnGUI()
    {
        if (!hasLost) return;

        float left = 0, right = Screen.width;
        float top = 0;

        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "You lost! You went into debt.");
        top += 24;

        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Money Spent: $" + moneysSpent);
        top += 24;
        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Enemies Killed: " + enemiesKilled);
        top += 24;
        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Enemies Banished: " + enemiesBanished);
        top += 24;
        GUI.Label(new Rect(left + 4, top + 4, right - left - 8, 40), "Time survived: " + (int)Mathf.Floor(timeSurvived) + "s");
        top += 24;

        if (GUI.Button(new Rect(left + 4, top + 4, right - left - 8, 40), "Restart")) restartGame();
        top += 24;
    }
    private void restartGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Destroy(gameObject);
    }
}
