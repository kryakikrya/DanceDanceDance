using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Battery : IDisposable
{
    private Image[] _segments;
    private Tween _batteryTween;
    private Tween[] _blinkTweens = new Tween[3];

    private const int EmptyHours = 3;
    private const int MinutesInDay = 24 * 60;
    private const float BlinkThreshold = 0.04f;
    private const float BlinkDuration = 0.2f;

    private bool _isDead;

    public float Charge { get; private set; } = 1f;

    public event Action OnBatteryDead;

    public Battery(ReallifeClock clock, Image[] segments)
    {
        _segments = segments;

        int currentMinutes = GetMinutes(clock.CurrentTime);
        int emptyMinutes = EmptyHours * 60;
        int remainingMinutes = (emptyMinutes - currentMinutes + MinutesInDay) % MinutesInDay;

        if (remainingMinutes == 0)
        {
            remainingMinutes = MinutesInDay;
        }

        _batteryTween = DOTween.To(
                () => Charge,
                value =>
                {
                    Charge = value;
                    UpdateSegments();
                },
                0f,
                remainingMinutes)
            .SetEase(Ease.Linear);
    }

    private void UpdateSegments()
    {
        for (int i = 0; i < _segments.Length; i++)
        {
            float threshold = 1f - (i + 1) / 3f;

            if (Charge <= threshold)
            {
                _blinkTweens[i]?.Kill();
                _blinkTweens[i] = null;
                _segments[i].enabled = false;

                if (i == _segments.Length - 1 && _isDead == false)
                {
                    _isDead = true;
                    OnBatteryDead?.Invoke();
                }

                continue;
            }

            if (Charge <= threshold + BlinkThreshold && _blinkTweens[i] == null)
            {
                _blinkTweens[i] = _segments[i]
                    .DOFade(0f, BlinkDuration)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }
    }

    private int GetMinutes(string time)
    {
        string[] parts = time.Split(':');

        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }

    public void Dispose()
    {
        _batteryTween?.Kill();

        foreach (Tween tween in _blinkTweens)
        {
            tween?.Kill();
        }
    }
}