using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ObstaclesObjectCreator : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnPosition;
    
    [SerializeField] private Vector2 _playerPosition;

    [SerializeField] private VisualizedObstacle _obstaclePrefab;
    
    [SerializeField] private float _duration;
    
    private Dictionary<int, VisualizedObstacle> _obstacles = new Dictionary<int, VisualizedObstacle>();
    
    private CheckCombination _checkCombination;

    private CancellationTokenSource _cts = new CancellationTokenSource();

    public float ObstacleFlyTime => _duration;

    [Inject]
    private void Construct(CheckCombination combinations)
    {
        _checkCombination = combinations;
    }

    private void OnEnable()
    {
        _checkCombination.OnObstacleCreated += CreateObstacle;
        _checkCombination.OnObstacleDestroyed += DeleteObstacle;
    }

    private void OnDisable()
    {
        _checkCombination.OnObstacleCreated -= CreateObstacle;
        _checkCombination.OnObstacleDestroyed -= DeleteObstacle;
        
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
    }

    private void CreateObstacle(Obstacle obstacle)
    {
        Debug.Log($"Добавляю Obstacle {obstacle.ID}");
        
        VisualizedObstacle newObstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
        _obstacles.Add(obstacle.ID, newObstacle);
        
        newObstacle.Animate(_spawnPosition, _playerPosition, _checkCombination.TimeAfter[CheckResultType.BadTime], _duration);
        newObstacle.SetID(obstacle.ID);
        
        newObstacle.LifetimeEnded += DeleteObstacle;
        
        newObstacle.Top.ChangeState(obstacle, true);
        newObstacle.Bottom.ChangeState(obstacle, false);
    }

    private void DeleteObstacle(int id)
    {
        Debug.Log($"Удаляю Obstacle {id}");
        
        if (!_obstacles.TryGetValue(id, out VisualizedObstacle obstacleObject))
        {
            return;
        }

        Destroy(obstacleObject.gameObject);
        _obstacles.Remove(id);
    }
    
    private void DeleteObstacle(Obstacle obstacle)
    {
        int id = obstacle.ID;
        
        Debug.Log($"Удаляю Obstacle {id}");
        
        if (!_obstacles.TryGetValue(id, out VisualizedObstacle obstacleObject))
        {
            return;
        }

        Destroy(obstacleObject.gameObject);
        _obstacles.Remove(id);
    }
}