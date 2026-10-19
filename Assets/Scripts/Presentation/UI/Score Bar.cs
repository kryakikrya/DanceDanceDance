using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private ScoreSystem _scoreSystem;

    [Inject]
    private void Construct(ScoreSystem scoreSystem)
    {
        _scoreSystem = scoreSystem;
    }

    private void OnEnable()
    {
        _scoreSystem.OnScoreChanged += UpdateScore;
        UpdateScore(_scoreSystem.Score);
    }

    private void OnDisable()
    {
        _scoreSystem.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(float score)
    {
        score = Mathf.RoundToInt(score);
        
        _scoreText.text = string.Concat(
            score.ToString().Select(c => $"<sprite name=\"{c}\">"));
    }
}