using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAchievement : MonoBehaviour, IAchievement
{
    private int timesJumped = 0;
    public PlayerMainController player;
    private void Start()
    {
        player.hasPerformedJump += CalculateJumps;
    }
    public void OnAchievement()
    {
        print("Jump achievement unlocked!");
        player.hasPerformedJump -= CalculateJumps;

    }

    void CalculateJumps()
    {
        timesJumped += 1;
        if (timesJumped >= 10) OnAchievement();
    }

    private void OnDestroy()
    {
        player.hasPerformedJump -= CalculateJumps;
    }
}
