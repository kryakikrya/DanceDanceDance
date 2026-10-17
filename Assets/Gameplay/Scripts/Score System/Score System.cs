using UnityEngine;

public class ScoreSystem
{
    private float _score = 0;
    
    private int _combination = 0;
    
    public float Score => _score;
    
    private ScoreSystemSettingsSO _settings;

    public ScoreSystem(ScoreSystemSettingsSO settings)
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
                _score -= _settings.TooEarlyTax;
                _score = Mathf.Clamp(_score, 0, float.MaxValue);
                break;
            case CheckResultType.TooLate:
                Debug.Log("Late");
                _combination = 0;
                _score -= _settings.TooLateTax;
                _score = Mathf.Clamp(_score, 0, float.MaxValue);
                break;
            case CheckResultType.WrongCombination:
                Debug.Log("Wrong Combination");
                _combination = 0;
                _score -= _settings.WrongCombinationTax;
                _score = Mathf.Clamp(_score, 0, float.MaxValue);
                break;
        }
    }
}
