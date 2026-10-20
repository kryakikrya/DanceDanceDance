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
    
    private ObstaclesObjectCreator  _obstaclesObjectCreator;
    
    private (DanceState topState, DanceState bottomState) _last;

    public ObstacleGeneration(SongClock clock, ObstaclesObjectCreator obstaclesObjectCreator)
    {
        _clock = clock;
        
        _obstaclesObjectCreator = obstaclesObjectCreator;
    }

    public void ChangeSong(SongSO song)
    {
        Stop();
        
        _duration = song.Duration;
        
        _timeForOneBeat = 60f / song.BPM;
        
        _obstacles = song.Beats;
    }

    public async UniTask PlaySong()
    {
        int index = 0;

        try
        {
            while (index < _obstacles.Length && GetTimeOfBeat(index) <= _duration)
            {
                float obstacleTime = GetTimeOfBeat(index);
                float creationTime = obstacleTime - _obstaclesObjectCreator.ObstacleFlyTime;

                Debug.Log($"Obstacle {index}: {obstacleTime} - {creationTime}");
                
                await UniTask.WaitUntil(
                    () => _clock.HasReached(creationTime),
                    cancellationToken: _cts.Token
                );

                CreateObstacle(index, obstacleTime);
                index++;
            }

            await UniTask.WaitUntil(
                () => _clock.HasReached(_duration),
                cancellationToken: _cts.Token
            );
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
        Debug.Log($"Obstacle {index}: {time}. Сейчас {_clock.SongTime}");
        
        DanceState topState;
        DanceState bottomState;
        
        do
        {
            topState = (DanceState)Random.Range(
                (int)DanceState.TopState1,
                (int)DanceState.TopState3 + 1
            );
            
            bottomState = (DanceState)Random.Range(
                (int)DanceState.BottomState1,
                (int)DanceState.BottomState3 + 1
            );
        } while (topState == _last.topState || bottomState == _last.bottomState);
        
        Debug.Log($"Current Combination {topState} {bottomState}");

        _last = (topState, bottomState);
        OnObstacleCreated?.Invoke(new Obstacle(index, topState, bottomState, time));
    }
    
    public void CreateObstacle(int index, float time, DanceState top, DanceState bottom)
    {
        OnObstacleCreated?.Invoke(new Obstacle(index, top, bottom, time));
    }
    
    public void Stop()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
    }
}