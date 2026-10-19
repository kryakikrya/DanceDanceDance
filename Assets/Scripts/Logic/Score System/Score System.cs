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
        float scoreBonus = _settings.TypeToHealh[result] * _settings.HealthToScore * (1 + (_combination * _settings.CombinationModifier));
        if (scoreBonus >= 0)
        {
            _score += scoreBonus;
            _combination++;
        }
        else
        {
            _combination = 0;
        }
        
        OnCheckResult?.Invoke(result);
        OnScoreChanged?.Invoke(_score);
    }
    
    public void Reset()
    {
        _score = 0;
    }
}
