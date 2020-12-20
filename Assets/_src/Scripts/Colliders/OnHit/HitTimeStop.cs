using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HitTimeStop : MonoBehaviour
{
    [SerializeField] private HitCheck mainHitBox;
    private float timeStopTimer;
    private float timeStopScale;

    private void Start()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit += ApplyEffect;
    }

    private void ApplyEffect(Vector3 pos, IDamageable hitBox)
    {
        if (hitBox == null)
            return;
        if (mainHitBox.HitProperties.timeStopLength == 0)
            return;

        timeStopTimer = mainHitBox.HitProperties.timeStopLength;
        timeStopScale = mainHitBox.HitProperties.timeStopScale;

        if(timeStopTimer > 0)
        {
            switch (mainHitBox.HitProperties.timeStopMode)
            {
                case HitProperties.TimeStopMode.SkipFrame:
                    StartCoroutine(SkipFrame());
                    break;
                case HitProperties.TimeStopMode.Delay:
                    StartCoroutine(Delay(mainHitBox.HitProperties.timeStopDelay));
                    break;
                default:
                    Stop();
                    break;
            }
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

    private IEnumerator SkipFrame()
    {
        yield return null;
        Stop();
        StopAllCoroutines();
    }

    private IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Stop();
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        if (mainHitBox == null)
            return;
        mainHitBox.OnSucessfulHit -= ApplyEffect;
    }
}
