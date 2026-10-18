using TMPro;
using UnityEngine;
using Zenject;

public class ClockView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    private ReallifeClock _clock;

    [Inject]
    private void Construct(ReallifeClock clock)
    {
        _clock = clock;
        _clock.OnTimeChanged += UpdateTime;

        UpdateTime(_clock.CurrentTime);
    }

    private void UpdateTime(string time)
    {
        _timeText.text = time;
    }

    private void OnDestroy()
    {
        _clock.OnTimeChanged -= UpdateTime;
    }
}