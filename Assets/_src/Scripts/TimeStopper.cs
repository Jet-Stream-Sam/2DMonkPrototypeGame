using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopper : MonoBehaviour
{
    private ControlManager controlManager;

    private void Start()
    {
        controlManager = ControlManager.Instance;
    }
    public void TimeStop()
    {
        controlManager.DisablePlayerControls(gameObject);
        PausingManager.Instance.PauseBlock(gameObject);
        Time.timeScale = 0;
    }

    public void TimeRelease()
    {
        if(!PausingManager.isGamePaused)
            Time.timeScale = 1;
        PausingManager.Instance.PauseUnblock(gameObject);
        controlManager.EnablePlayerControls(gameObject);
    }
}
