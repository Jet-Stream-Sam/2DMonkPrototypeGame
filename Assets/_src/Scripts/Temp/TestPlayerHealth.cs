using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerHealth : MonoBehaviour
{
    public TestPlayerHealthData playerHealthData;

    private void Start()
    {
        playerHealthData.ResetHealth();
        print("Health: " + playerHealthData.CurrentHealth + "/" + playerHealthData.MaxHealth);
        playerHealthData.TakeDamage(20);
        print("Health: " + playerHealthData.CurrentHealth + "/" + playerHealthData.MaxHealth);
        
    }
}
