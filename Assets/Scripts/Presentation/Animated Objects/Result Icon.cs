using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class ResultIcon : MonoBehaviour
{
    [SerializeField] private Dictionary<CheckResultType, ResultSpriteAndSound> _spriteAndSound = new();
    [SerializeField] private SpriteRenderer _resultSprite;
    [SerializeField] private float _appearDuration = 0.3f;
    [SerializeField] private float _displayDuration = 2f;
    [SerializeField] private float _disappearDuration = 0.3f;
    [SerializeField] private float _rotationAngle = 360f;
    [SerializeField] private Ease _appearEase = Ease.OutBack;
    [SerializeField] private Ease _disappearEase = Ease.InBack;

    private Coroutine _displayCoroutine;
    private Sequence _animation;
    private SoundSystem _soundSystem;
    private Vector3 _initialScale;
    private Quaternion _initialRotation;

    [Inject]
    private void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Awake()
    {
        _initialScale = _resultSprite.transform.localScale;
        _initialRotation = _resultSprite.transform.localRotation;
        _resultSprite.enabled = false;
    }

    public void DoResult(CheckResultType result)
    {
        if (!_spriteAndSound.TryGetValue(result, out ResultSpriteAndSound resultData))
        {
            return;
        }

        if (_displayCoroutine != null)
        {
            StopCoroutine(_displayCoroutine);
        }

        _animation?.Kill();

        Transform iconTransform = _resultSprite.transform;

        _resultSprite.sprite = resultData.Sprite;
        _resultSprite.enabled = true;
        iconTransform.localScale = Vector3.zero;
        iconTransform.localRotation = _initialRotation;

        _displayCoroutine = StartCoroutine(DisplayCoroutine(resultData.SoundPath));
    }

    private IEnumerator DisplayCoroutine(string sound)
    {
        _soundSystem.PlaySound(sound);

        Transform iconTransform = _resultSprite.transform;

        _animation = DOTween.Sequence()
            .Append(iconTransform.DOScale(_initialScale, _appearDuration).SetEase(_appearEase))
            .Join(iconTransform.DOLocalRotateQuaternion(
                _initialRotation * Quaternion.Euler(0f, 0f, _rotationAngle),
                _appearDuration).SetEase(_appearEase));

        yield return _animation.WaitForCompletion();
        yield return new WaitForSeconds(_displayDuration);

        _animation = DOTween.Sequence()
            .Append(iconTransform.DOScale(Vector3.zero, _disappearDuration).SetEase(_disappearEase))
            .Join(iconTransform.DOLocalRotateQuaternion(
                _initialRotation,
                _disappearDuration).SetEase(_disappearEase));

        yield return _animation.WaitForCompletion();

        _resultSprite.enabled = false;
        iconTransform.localScale = _initialScale;
        iconTransform.localRotation = _initialRotation;
        _displayCoroutine = null;
    }

    private void OnDestroy()
    {
        _animation?.Kill();
    }
}

[Serializable]
public class ResultSpriteAndSound
{
    public string SoundPath;
    public Sprite Sprite;
}