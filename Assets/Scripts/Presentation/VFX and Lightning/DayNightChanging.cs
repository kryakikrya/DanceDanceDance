using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightChanging
{
    private const int MinutesInDay = 24 * 60;
    private const int NightHours = 3;
    private const float TransitionDuration = 1f;

    private readonly ReallifeClock _clock;
    private readonly Light2D _globalLight;
    private readonly Bloom _bloom;

    private Tween _lightTween;
    private Tween _bloomTween;

    private float _initialLightIntensity;
    private float _initialBloomIntensity;

    public float TargetLightIntensity { get; set; } = 0.02f;
    public float TargetBloomIntensity { get; set; } = 1f;

    public DayNightChanging(ReallifeClock clock, Light2D globalLight, Volume volume)
    {
        _clock = clock;
        _globalLight = globalLight;

        volume.profile.TryGet(out _bloom);

        _initialLightIntensity = _globalLight.intensity;
        _initialBloomIntensity = _bloom.intensity.value;

        _clock.OnTimeChanged += UpdateLighting;

        UpdateLighting(_clock.CurrentTime);
    }

    private void UpdateLighting(string currentTime)
    {
        int currentMinutes = GetMinutes(currentTime);
        int startMinutes = GetMinutes(_clock.StartTime);
        int targetMinutes = NightHours * 60;

        int totalMinutes = (targetMinutes - startMinutes + MinutesInDay) % MinutesInDay;
        int elapsedMinutes = (currentMinutes - startMinutes + MinutesInDay) % MinutesInDay;

        if (totalMinutes == 0)
        {
            totalMinutes = MinutesInDay;
        }

        float progress = Mathf.Clamp01((float)elapsedMinutes / totalMinutes);

        float targetLight = Mathf.Lerp(_initialLightIntensity, TargetLightIntensity, progress);
        float targetBloom = Mathf.Lerp(_initialBloomIntensity, TargetBloomIntensity, progress);

        _lightTween?.Kill();
        _bloomTween?.Kill();

        _lightTween = DOTween.To(
            () => _globalLight.intensity,
            value => _globalLight.intensity = value,
            targetLight,
            TransitionDuration);

        _bloomTween = DOTween.To(
            () => _bloom.intensity.value,
            value => _bloom.intensity.value = value,
            targetBloom,
            TransitionDuration);
    }

    private int GetMinutes(string time)
    {
        string[] parts = time.Split(':');

        return int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
    }

    public void Dispose()
    {
        _clock.OnTimeChanged -= UpdateLighting;
        _lightTween?.Kill();
        _bloomTween?.Kill();
    }
}