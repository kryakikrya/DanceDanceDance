using System;
using UnityEngine;

public class HealthSystem
{
    private float _maxHealth;
    private float _currentHealth;
    
    private CombinationSettingsSO _settings;

    public event Action OnPlayerDead;
    
    public event Action OnHealthChanged;
    
    public float CurrentHealth => _currentHealth;
    
    public float MaxHealth => _maxHealth;
    
    public HealthSystem(int maxHealth, CombinationSettingsSO settings)
    {
        _settings = settings;
        
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }
    
    public void ChangeHealth(CheckResultType result)
    {
        _currentHealth += _settings.TypeToHealh[result];
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        
        Debug.Log($"Current Health - {_currentHealth}");
        
        OnHealthChanged?.Invoke();
        
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            Death();
        }
    }

    public void ResetHealth()
    {
        _currentHealth = _maxHealth;
    }

    private void Death()
    {
        Debug.Log("OnPLayerDead");
        
        OnPlayerDead?.Invoke();

        ResetHealth();
    }
}