using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVitalityMeterRecovery : MonoBehaviour
{
    [SerializeField] private PlayerMainController playerMainController;
    [SerializeField] private int rateOverTime = 25;
    [SerializeField] private float recoveryInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(RecoverOverTime());
    }

    private IEnumerator RecoverOverTime()
    {
        while (true)
        {
            playerMainController.FillVitalityOrbMeter(rateOverTime);
            yield return new WaitForSeconds(recoveryInterval);

        }
    }
}
