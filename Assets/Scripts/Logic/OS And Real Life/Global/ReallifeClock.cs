using System;
using System.Threading;
using Cysharp.Threading.Tasks;

public class ReallifeClock : IDisposable
{
    private CancellationTokenSource _cts;
    
    private int _startHours;
    private int _startMinutes;

    private int _hours;
    private int _minutes;

    public string CurrentTime => $"{_hours:D2}:{_minutes:D2}";
    
    public string StartTime => $"{_startHours:D2}:{_startMinutes:D2}";
    
    public event Action<string> OnTimeChanged;

    public ReallifeClock(int startHours, int startMinutes)
    {
        _cts = new CancellationTokenSource();
        
        _startHours = startHours;
        _startMinutes = startMinutes;
        
        _hours = _startHours;
        _minutes = _startMinutes;

        UpdateTimeAsync().Forget();
    }

    private async UniTaskVoid UpdateTimeAsync()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(1f, cancellationToken: _cts.Token);

            _minutes++;

            if (_minutes >= 60)
            {
                _minutes = 0;
                _hours++;

                if (_hours >= 24)
                {
                    _hours = 0;
                }
            }
            
            OnTimeChanged?.Invoke(CurrentTime);
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
    }
}