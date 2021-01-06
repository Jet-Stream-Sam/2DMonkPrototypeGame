using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerMeterRecovery : MonoBehaviour
{
    [SerializeField] private PlayerMainController playerMainController;
    [SerializeField] private int rateOnHit = 25;

    private void Start()
    {
        playerMainController.hitBoxCheck.OnSucessfulHit += (A, B) => RecoverOnHit();
    }

    private void RecoverOnHit()
    {
        playerMainController.FillPowerOrbMeter(rateOnHit);
    }
}
