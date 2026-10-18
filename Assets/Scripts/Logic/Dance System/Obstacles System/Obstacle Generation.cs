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

    public ObstacleGeneration(SongClock clock, ObstaclesObjectCreator obstaclesObjectCreator)
    {
        _clock = clock;
        
        _obstaclesObjectCreator = obstaclesObjectCreator;
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

        try
        {
            while (index < _obstacles.Length && GetTimeOfBeat(index) <= _duration)
            {
                float obstacleTime = GetTimeOfBeat(index);
                float creationTime = obstacleTime - _obstaclesObjectCreator.ObstacleFlyTime;

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