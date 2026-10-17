using System;
using UnityEngine;

public class ScoreSystem
{
    private float _score = 0;
    
    private int _combination = 0;
    
    public float Score => _score;
    
    private CombinationSettingsSO _settings;
    
    public event Action<CheckResultType> OnCheckResult;
    public event Action<float> OnScoreChanged;

    public ScoreSystem(CombinationSettingsSO settings)
    {
        _settings = settings;
    }
    
    public void AddScore(CheckResultType result)
    {
        switch (result)
        {
            case CheckResultType.PerfectTime:
                Debug.Log("Perfect");
                _combination++;
                _score += _settings.PerfectBonus * (1 + (_settings.CombinationModifier * _combination));
                break;
            case CheckResultType.TooEarly:
                Debug.Log("Early");
                _combination = 0;
                break;
            case CheckResultType.TooLate:
                Debug.Log("Late");
                _combination = 0;
                break;
            case CheckResultType.WrongCombination:
                Debug.Log("Wrong Combination");
                _combination = 0;
                break;
        }
        
        OnCheckResult?.Invoke(result);
        OnScoreChanged?.Invoke(_score);
    }
}
