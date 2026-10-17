using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ObstaclesObjectCreator : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnPosition;
    
    [SerializeField] private Vector2 _playerPosition;

    [SerializeField] private VisualizedObstacle _obstaclePrefab;
    
    private Dictionary<int, VisualizedObstacle> _obstacles = new Dictionary<int, VisualizedObstacle>();
    
    private CheckCombination _checkCombination;

    private CancellationTokenSource _cts = new CancellationTokenSource();

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
        VisualizedObstacle newObstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
        _obstacles.Add(obstacle.ID, newObstacle);
        
        newObstacle.Animate(_spawnPosition, _playerPosition, _checkCombination.TimeAfter);
        newObstacle.Top.ChangeState(obstacle, true);
        newObstacle.Bottom.ChangeState(obstacle, false);
    }

    private void DeleteObstacle(Obstacle obstacle)
    {
        if (!_obstacles.TryGetValue(obstacle.ID, out VisualizedObstacle gameObject))
        {
            return;
        }

        Destroy(gameObject);
        _obstacles.Remove(obstacle.ID);
    }
}