using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAsyncCountdown : MonoBehaviour
{
    private AsyncCountdown countdown;
    private bool trigger = false;
    private void Start()
    {
        
    }

    private void Update()
    {
        if(Time.time > 10 && !trigger)
        {
            trigger = true;
            countdown = new AsyncCountdown(30);
            countdown.StartCountdown();

        }
        else if(Time.time > 10)
        {
            Debug.Log(countdown.countSeconds);
        }
    }
}
