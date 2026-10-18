using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HealthIcon : MonoBehaviour
{
    [SerializeField] private Image _fill;

    [Inject] private HealthSystem _healthSystem;

    private void OnEnable()
    {
        _healthSystem.OnHealthChanged += UpdateHealth;
        UpdateHealth();
    }

    private void OnDisable()
    {
        _healthSystem.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth()
    {
        _fill.fillAmount = _healthSystem.CurrentHealth / _healthSystem.MaxHealth;
    }
}