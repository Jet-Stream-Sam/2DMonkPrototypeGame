
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(menuName = "Scriptable Objects/Test/PlayerHealthData")]
public class TestPlayerHealthData : ScriptableObject
{
    
    [SerializeField] private float _maxHealth;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth { get; private set; }
    public float HealthPercentage => CurrentHealth * 100 / _maxHealth;

    public void ResetHealth()
    {
        CurrentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            KillCharacter();
        }
    }

    private void KillCharacter()
    {
        Debug.Log("The character is dead!");
    }

    
    
}
