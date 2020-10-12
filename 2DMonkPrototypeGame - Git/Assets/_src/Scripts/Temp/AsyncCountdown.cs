using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncCountdown
{
    float seconds;
    public float countSeconds;
    public async void StartCountdown()
    {
        countSeconds = seconds;

        while (countSeconds > 0)
        {
            countSeconds -= Time.unscaledDeltaTime;
            
            await Task.Yield();
        }   
    }
    public AsyncCountdown(float seconds)
    {
        this.seconds = seconds;
    }
}
