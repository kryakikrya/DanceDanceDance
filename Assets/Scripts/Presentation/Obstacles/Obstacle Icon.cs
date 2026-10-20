using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ObstacleIcon : MonoBehaviour, IObstacleVisualization
{
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _spriteRenderer2;
        
    private Coroutine _animationCoroutine;
    private Tween _movementTween;
    
    public event Action<int> LifetimeEnded;

    public void ChangeIcon(Sprite sprite)
    {
        Debug.Log("Icon Change");
        _renderer.sprite = sprite;
    }
    
    private int _id;
    public int ID => _id;

    public void SetID(int id)
    {
        _id = id;
    }

    public void Animate(
        Vector2 spawnPosition,
        Vector2 playerPosition,
        float timeAfter,
        float duration)
    {
        Debug.Log("Icon Start");
        _movementTween?.Kill();

        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }

        transform.position = spawnPosition;

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
        Debug.Log("Coroutine Start");

        _movementTween = transform.DOMove(playerPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Debug.Log("Obstacle reached player position"));

        yield return _movementTween.WaitForCompletion();

        Vector2 direction = (playerPosition - spawnPosition).normalized;
        float speed = Vector2.Distance(spawnPosition, playerPosition) / duration;

        Vector2 targetPosition = playerPosition + direction * speed * timeAfter;

        _movementTween = DOTween.Sequence()
            .Append(transform.DOMove(targetPosition, timeAfter)
                .SetEase(Ease.Linear))
            .Join(_spriteRenderer.DOFade(0f, timeAfter)
                .SetEase(Ease.Linear))
            .Join(_spriteRenderer2.DOFade(0f, timeAfter)
            .SetEase(Ease.Linear));

        yield return _movementTween.WaitForCompletion();

        _animationCoroutine = null;
        LifetimeEnded?.Invoke(_id);
    }
    
    public async UniTask AnimationAsync(
        Vector2 playerPosition,
        float duration)
    {
        Debug.Log("Animation Start");

        _movementTween = transform.DOMove(playerPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => Debug.Log("Obstacle reached player position"));

        await _movementTween.AsyncWaitForCompletion();
    }
    
    public void OnDestroy()
    {
        _movementTween?.Kill();

        if (gameObject != null)
        {
            Destroy(gameObject);   
        }
    }
}
