using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public Text DateDisplay;

    public float SecondsPerDay = 60;
    public int startingYear = 2200;
    public DateTime gameDay;

    public delegate void Tick();
    public static event Tick OnDayTick;

    private float tickTime = 0;

    private void Awake()
    {
        gameDay = new DateTime(startingYear, 1, 1);
        tickTime = 0;
        DateDisplay.text = gameDay.ToString("MM/dd/yyyy");
    }
    
    private void Update()
    {
        tickTime += Time.deltaTime;
        if (tickTime >= SecondsPerDay)
        {
            gameDay += new TimeSpan(1, 0, 0, 0);
            tickTime = 0;
            DateDisplay.text = gameDay.ToString("MM/dd/yyyy");
            OnDayTick();
        }
    }
}
