using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSystemSettingsSO", menuName = "Scriptable Objects/ScoreSystemSettingsSO")]
public class ScoreSystemSettingsSO : ScriptableObject
{
    [SerializeField] private float _perfectBonus;
    public float PerfectBonus => _perfectBonus;
    
    [SerializeField] private float _wrongCombinationTax;
    public float WrongCombinationTax => _wrongCombinationTax;
    
    [SerializeField] private float _tooLateTax;
    public float TooLateTax => _tooLateTax;
    
    [SerializeField] private float _tooEarlyTax;
    public float TooEarlyTax => _tooEarlyTax;
    
    [SerializeField] private float _combinationModifier;
    public float CombinationModifier => _combinationModifier;
}
