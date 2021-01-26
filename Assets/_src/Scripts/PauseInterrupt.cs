using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseInterrupt : MonoBehaviour
{
    public void InterruptPause()
    {
        PausingManager.canPause = false;
    }

    public void ContinuePause()
    {
        PausingManager.canPause = true;
    }
}
