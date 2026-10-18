using DG.Tweening;
using UnityEngine;

public class VisualizedObstacle : MonoBehaviour
{
    [field: SerializeField] public PlayerMaskablePart Top { get; private set; }
    [field: SerializeField] public PlayerMaskablePart Bottom { get; private set; }
    
    [SerializeField] private int _layer = 20;
    [SerializeField] private float _sizeFrom;
    [SerializeField] private float _sizeTo;
    [SerializeField] private float _duration;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Coroutine _animationCoroutine;
    private Tween _movementTween;
    private Tween _scaleTween;

    public void Animate(Vector2 spawnPosition, Vector2 playerPosition, float timeAfter)
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        _movementTween?.Kill();
        _scaleTween?.Kill();

        _animationCoroutine = StartCoroutine(
            AnimationCoroutine(spawnPosition, playerPosition, timeAfter)
        );
    }

    private System.Collections.IEnumerator AnimationCoroutine(
        Vector2 spawnPosition,
        Vector2 playerPosition,
        float timeAfter)
    {
        transform.position = spawnPosition;
        transform.localScale = Vector3.one * _sizeFrom;

        _movementTween = transform.DOMove(playerPosition, _duration)
            .SetEase(Ease.Linear);

        _scaleTween = transform.DOScale(_sizeTo, _duration)
            .SetEase(Ease.Linear);

        while (_movementTween.IsActive() && !_movementTween.IsComplete())
        {
            yield return null;
        }

        _spriteRenderer.sortingOrder = _layer;

        Vector2 direction = (playerPosition - spawnPosition).normalized;
        float speed = (playerPosition - spawnPosition).magnitude / _duration;

        _movementTween = transform.DOMove(
                (Vector2)playerPosition + direction * speed * timeAfter,
                timeAfter)
            .SetEase(Ease.Linear);

        while (_movementTween.IsActive() && !_movementTween.IsComplete())
        {
            yield return null;
        }

        _animationCoroutine = null;
    }

    private void OnDestroy()
    {
        _movementTween?.Kill();
        _scaleTween?.Kill();
    }
}