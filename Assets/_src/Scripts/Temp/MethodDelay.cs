using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class MethodDelay
{
    public async static void DelayMethodByTimeASync(Action action, float timeToWait)
    {
        await Task.Delay((int)(timeToWait * 1000));
        
        action?.Invoke();

    }
}
