using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleGeneration
{
    private CancellationTokenSource _cts = new CancellationTokenSource();

    private float _duration;
    private float _timeForOneBeat;

    private int[] _obstacles;

    public event Action<Obstacle> OnObstacleCreated;

    private SongClock _clock;

    public ObstacleGeneration(SongClock clock)
    {
        _clock = clock;
    }

    public void ChangeSong(SongSO song)
    {
        _duration = song.Duration;
        
        _timeForOneBeat = 60f / song.BPM;
        
        _obstacles = song.Beats;
    }

    public async UniTask PlaySong()
    {
        int index = 0;

        CreateObstacle(index, GetTimeOfBeat(index));

        try
        {
            while (!_clock.HasReached(_duration))
            {
                float targetTime = _duration;

                if (index < _obstacles.Length)
                {
                    targetTime = _obstacles[index] * _timeForOneBeat;

                    if (targetTime > _duration)
                    {
                        targetTime = _duration;
                    }
                }

                await UniTask.WaitUntil(
                    () => _clock.HasReached(targetTime),
                    cancellationToken: _cts.Token
                );

                if (index < _obstacles.Length && _clock.HasReached(GetTimeOfBeat(index)))
                {
                    //Debug.Log($"SongTime {_clock.SongTime}");
                    index++;
                    CreateObstacle(index, GetTimeOfBeat(index));
                }
            }
        }
        catch (OperationCanceledException)
        {
            return;
        }
    }

    public float GetTimeOfBeat(int index)
    {
        return _obstacles[index] * _timeForOneBeat;
    }

    private void CreateObstacle(int index, float time)
    {
        DanceState topState = (DanceState)Random.Range(
            (int)DanceState.TopState1,
            (int)DanceState.TopState3 + 1
        );

        DanceState bottomState = (DanceState)Random.Range(
            (int)DanceState.BottomState1,
            (int)DanceState.BottomState3 + 1
        );
        
        Debug.Log($"Current Combination {topState} {bottomState}");

        OnObstacleCreated?.Invoke(new Obstacle(index, topState, bottomState, time));
    }
}