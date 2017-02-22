using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public Scene nextScene;
    public float secondsUntilNavigation;

    private float navigationTime;

    void Start()
    {
        navigationTime = Time.time + secondsUntilNavigation;
    }
    
    void Update()
    {
        if (Time.time > navigationTime)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}
