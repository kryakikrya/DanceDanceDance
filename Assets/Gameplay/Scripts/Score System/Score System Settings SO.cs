using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSystemSettingsSO", menuName = "Scriptable Objects/ScoreSystemSettingsSO")]
public class CombinationSettingsSO : ScriptableObject
{
    [SerializeField] private float _perfectBonus;
    public float PerfectBonus => _perfectBonus;
    
    [SerializeField] private float _perfectHPBonus;
    public float PerfectHPBonus => _perfectHPBonus;
    
    [SerializeField] private float _wrongCombinationHPTax;
    public float WrongCombinationTax => _wrongCombinationHPTax;
    
    [SerializeField] private float _tooLateHPTax;
    public float TooLateTax => _tooLateHPTax;
    
    [SerializeField] private float _tooEarlyHPTax;
    public float TooEarlyTax => _tooEarlyHPTax;
    
    [SerializeField] private float _combinationModifier;
    public float CombinationModifier => _combinationModifier;
}
