using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class ScoreEndGame : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private string _sound;

    private ScoreSystem _scoreSystem;
    private SoundSystem _soundSystem;
    private Tween _scoreTween;

    [Inject]
    private void Construct(ScoreSystem scoreSystem, SoundSystem soundSystem)
    {
        _scoreSystem = scoreSystem;
        _soundSystem = soundSystem;
    }

    private void OnEnable()
    {
        float targetScore = _scoreSystem.Score;
        int displayedScore = 0;
        float animatedScore = 0f;

        _scoreText.text = "0";

        _scoreTween = DOTween.To(
            () => animatedScore,
            value =>
            {
                animatedScore = value;

                int nextScore = Mathf.RoundToInt(value);

                if (nextScore == displayedScore)
                {
                    return;
                }

                displayedScore = nextScore;
                _scoreText.text = displayedScore.ToString();
                _soundSystem.PlaySound(_sound);
            },
            targetScore,
            _duration
        ).SetEase(Ease.OutExpo);
    }

    private void OnDisable()
    {
        _scoreTween?.Kill();
    }
}