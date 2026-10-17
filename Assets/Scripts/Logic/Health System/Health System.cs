using System;
using UnityEngine;

public class HealthSystem
{
    private float _maxHealth;
    private float _currentHealth;
    
    private CombinationSettingsSO _settings;

    private event Action OnPlayerDead;
    
    public HealthSystem(int maxHealth, CombinationSettingsSO settings)
    {
        _settings = settings;
        
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }
    
    public void ChangeHealth(CheckResultType result)
    {
        switch (result)
        {
            case CheckResultType.PerfectTime:
                _currentHealth += _settings.PerfectHPBonus;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                break;
            case CheckResultType.TooEarly:
                _currentHealth -= _settings.TooEarlyTax;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                break;
            case CheckResultType.TooLate:
                _currentHealth -= _settings.TooLateTax;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                break;
            case CheckResultType.WrongCombination:
                _currentHealth -= _settings.WrongCombinationTax;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
                break;
        }
        
        Debug.Log($"Current Health - {_currentHealth}");
        
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;

            Death();
        }
    }

    private void Death()
    {
        Debug.Log("OnPLayerDead");
        
        OnPlayerDead?.Invoke();
    }
}