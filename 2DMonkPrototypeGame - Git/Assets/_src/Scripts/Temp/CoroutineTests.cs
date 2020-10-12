using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineTests : MonoBehaviour
{
    IEnumerator currentShootCoroutine;
    [SerializeField] private float timeBetweenCasts;
    [SerializeField] private float shotsAmount;

    private void Start()
    {
        BurstCast();
    }
    private void BurstCast()
    {
        if(currentShootCoroutine != null)
        {
            StopCoroutine(currentShootCoroutine);
        }

        currentShootCoroutine = BurstCastRoutine();
        StartCoroutine(currentShootCoroutine);

        IEnumerator BurstCastRoutine()
        {
            for (int i = 0; i < shotsAmount; i++)
            {
                print("Shoot!");

                yield return new WaitForSeconds(timeBetweenCasts);
            }
        }

        
    }
}
