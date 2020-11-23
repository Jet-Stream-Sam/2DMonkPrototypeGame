using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TimeStop : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    private float timeStopTimer;
    private float timeStopScale;

    private void Start()
    {
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        timeStopTimer = mainHitBox.HitProperties.timeStopLength;
        timeStopScale = mainHitBox.HitProperties.timeStopScale;

        if(timeStopTimer > 0)
        {
            Stop();
        }
    }

    private async void Stop()
    {
        Time.timeScale = timeStopScale;

        while(timeStopTimer > 0)
        {
            timeStopTimer -= Time.unscaledDeltaTime;
            await Task.Yield();
        }

        if(!PausingManager.isGamePaused)
            Time.timeScale = 1f;

    }
    private void OnDestroy()
    {
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
