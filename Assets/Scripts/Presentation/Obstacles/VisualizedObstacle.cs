using System;
using DG.Tweening;
using UnityEngine;

public class VisualizedObstacle : MonoBehaviour, IObstacleVisualization
{
    [field: SerializeField] public PlayerMaskablePart Top { get; private set; }
    [field: SerializeField] public PlayerMaskablePart Bottom { get; private set; }
    
    [SerializeField] private int _layer = 20;
    [SerializeField] private float _sizeFrom;
    [SerializeField] private float _sizeTo;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Coroutine _animationCoroutine;
    private Tween _movementTween;
    private Tween _scaleTween;

    public event Action<int> LifetimeEnded;

    private int _id;
    public int ID => _id;

    public void SetID(int id)
    {
        _id = id;
    }

    public void Animate(Vector2 spawnPosition, Vector2 playerPosition, float timeAfter, float duration)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _movementTween?.Kill();
        _scaleTween?.Kill();

        _animationCoroutine = StartCoroutine(
            AnimationCoroutine(spawnPosition, playerPosition, timeAfter, duration)
        );
    }
    
    private System.Collections.IEnumerator AnimationCoroutine(
        Vector2 spawnPosition,
        Vector2 playerPosition,
        float timeAfter,
        float duration)
    {
        transform.position = spawnPosition;
        transform.localScale = Vector3.one * _sizeFrom;

        float initialAlpha = _spriteRenderer.color.a;
        _spriteRenderer.color = new Color(
            _spriteRenderer.color.r,
            _spriteRenderer.color.g,
            _spriteRenderer.color.b,
            0f
        );

        _spriteRenderer.DOFade(initialAlpha, 0.5f)
            .SetEase(Ease.Linear);

        Vector2 direction = (playerPosition - spawnPosition).normalized;
        float distance = Vector2.Distance(spawnPosition, playerPosition);
        float speed = distance / duration;
        float totalDuration = duration + timeAfter;

        Vector2 endPosition = playerPosition + direction * speed * timeAfter;

        float endSize = _sizeFrom +
                        (_sizeTo - _sizeFrom) * (totalDuration / duration);

        _movementTween = DOTween.Sequence()
            .Append(transform.DOMove(endPosition, totalDuration)
                .SetEase(Ease.Linear))
            .Insert(duration, _spriteRenderer.DOFade(0f, timeAfter)
                .SetEase(Ease.Linear))
            .InsertCallback(duration, () => _spriteRenderer.sortingOrder = _layer);

        _scaleTween = transform.DOScale(endSize, totalDuration)
            .SetEase(Ease.Linear);

        while (_movementTween.IsActive() && !_movementTween.IsComplete())
        {
            yield return null;
        }

        _animationCoroutine = null;

        LifetimeEnded?.Invoke(_id);
    }

    public void OnDestroy()
    {
        _movementTween?.Kill();
        _scaleTween?.Kill();
        Destroy(gameObject);
    }
}