using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Zenject;

public class ObstaclesObjectCreator : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnPosition;
    
    [SerializeField] private Vector2 _playerPosition;
    
    [SerializeField] private Vector2 _iconStartPosition;
    
    [SerializeField] private Vector2 _iconEndPosition;

    [SerializeField] private VisualizedObstacle _obstaclePrefab;
    [SerializeField] private ObstacleIcon _obstacleIconPrefab;
    
    [SerializeField] private float _duration;
    
    [SerializeField] private IconsSO _iconsSO;
    
    [Inject] private HealthSystem _healthSystem;
    
    private Dictionary<int, (IObstacleVisualization, IObstacleVisualization)> _obstacles = new Dictionary<int, (IObstacleVisualization, IObstacleVisualization)>();
    
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

        DeleteAll();
    }

    private void CreateObstacle(Obstacle obstacle)
    {
        Debug.Log($"Добавляю Obstacle {obstacle.ID}");
        
        VisualizedObstacle newObstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
        newObstacle.Top.ChangeState(obstacle, true);
        newObstacle.Bottom.ChangeState(obstacle, false);
        
        AddVisualization(newObstacle, obstacle, _spawnPosition, _playerPosition);
        
        ObstacleIcon icon = Instantiate(_obstacleIconPrefab, _iconStartPosition, Quaternion.identity);
        icon.ChangeIcon(_iconsSO.GetIcon(new DancePair {Top = obstacle.TopState, Bottom = obstacle.BottomState}));
        
        AddVisualization(icon, obstacle, _iconStartPosition, _iconEndPosition);
        
        _obstacles.Add(obstacle.ID, (newObstacle, icon));
    }

    private void AddVisualization(IObstacleVisualization newObstacle, Obstacle obstacle, Vector2 pos1, Vector2 pos2)
    {
        Debug.Log($"Visualized Obstacle {newObstacle}");
        
        newObstacle.Animate(pos1, pos2, _checkCombination.TimeAfter[CheckResultType.BadTime], _duration);
        newObstacle.SetID(obstacle.ID);
        
        newObstacle.LifetimeEnded += DeleteObstacle;
    }

    private void DeleteObstacle(int id)
    {
        Debug.Log($"Удаляю Obstacle {id}");
        
        if (!_obstacles.TryGetValue(id, out (IObstacleVisualization, IObstacleVisualization) obstacleObject))
        {
            return;
        }
        
        _obstacles.Remove(id);
        obstacleObject.Item1.OnDestroy();
        obstacleObject.Item1.LifetimeEnded -= DeleteObstacle;
        obstacleObject.Item2.OnDestroy();
        obstacleObject.Item2.LifetimeEnded -= DeleteObstacle;
    }
    
    private void DeleteObstacle(Obstacle obstacle)
    {
        int id = obstacle.ID;
        
        Debug.Log($"Удаляю Obstacle {id}");
        
        if (!_obstacles.TryGetValue(id, out (IObstacleVisualization, IObstacleVisualization) obstacleObject))
        {
            return;
        }
        
        _obstacles.Remove(id);
        
        _healthSystem.ChangeHealth(CheckResultType.TooLate);
        
        obstacleObject.Item1.LifetimeEnded -= DeleteObstacle;
        obstacleObject.Item1.OnDestroy();
        obstacleObject.Item2.LifetimeEnded -= DeleteObstacle;
        obstacleObject.Item2.OnDestroy();
    }

    public void DeleteAll()
    {
        foreach (KeyValuePair<int, (IObstacleVisualization, IObstacleVisualization)> obstacle in _obstacles)
        {
            DeleteObstacle(obstacle.Key);
        }
    }
}