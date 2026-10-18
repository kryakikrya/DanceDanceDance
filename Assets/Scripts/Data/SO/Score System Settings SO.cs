using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSystemSettingsSO", menuName = "Scriptable Objects/ScoreSystemSettingsSO")]
public class CombinationSettingsSO : ScriptableObject
{
    [SerializeField] private Dictionary<CheckResultType, float> _typeToHealth = new Dictionary<CheckResultType, float>();
    public Dictionary<CheckResultType, float> TypeToHealh => _typeToHealth;
    
    [SerializeField] private float _healthToScore;
    public float HealthToScore => _healthToScore;
    
    
    [SerializeField] private float _combinationModifier;
    public float CombinationModifier => _combinationModifier;
}
