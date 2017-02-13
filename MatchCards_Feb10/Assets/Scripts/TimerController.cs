using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    public float startTime = 60 * 5; //Five minutes
    public Text txtTimer;
    private float timeLeft = 0;

    public bool IsOutOfTime
    {
        get
        {
            return timeLeft <= 0;
        }
    }

    void Start()
    {
        timeLeft = startTime;
    }
    
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;
        if (txtTimer != null)
        {
            if (IsOutOfTime) txtTimer.text = "Out of time!";
            else
            {
                int minutes = (int)Math.Floor(timeLeft / 60);
                String seconds = Math.Floor(timeLeft - (minutes * 60)).ToString();
                txtTimer.text = minutes.ToString() + ":" + (seconds.Length == 1 ? "0" : "") + seconds + " Left!";
            }
        }
    }
}
