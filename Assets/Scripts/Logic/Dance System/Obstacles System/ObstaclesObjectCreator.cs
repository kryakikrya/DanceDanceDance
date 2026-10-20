using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
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

    private SongClock _clock;

    [Inject]
    private void Construct(CheckCombination combinations, SongClock clock)
    {
        _checkCombination = combinations;

        _clock = clock;
    }

    private void OnEnable()
    {
        _checkCombination.OnObstacleCreated += CreateObstacle;
        _checkCombination.OnObstacleDestroyed += DeleteObstacle;

        if (_cts is null)
        {
            _cts = new CancellationTokenSource();
        }
    }

    private void OnDisable()
    {
        _checkCombination.OnObstacleCreated -= CreateObstacle;
        _checkCombination.OnObstacleDestroyed -= DeleteObstacle;

        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;   
        }

        DeleteAll();
    }

    private void CreateObstacle(Obstacle obstacle)
    {
        Debug.Log($"Добавляю Obstacle {obstacle.ID} {obstacle.Time} {obstacle.BottomState} {obstacle.TopState}");
        
        VisualizedObstacle newObstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
        newObstacle.Top.ChangeState(obstacle, true);
        newObstacle.Bottom.ChangeState(obstacle, false);
        
        AddVisualization(newObstacle, obstacle, _spawnPosition, _playerPosition);
        
        ObstacleIcon icon = Instantiate(_obstacleIconPrefab, _iconStartPosition, Quaternion.identity);
        icon.ChangeIcon(_iconsSO.GetIcon(new DancePair {Top = obstacle.TopState, Bottom = obstacle.BottomState}));
        
        AddVisualization(icon, obstacle, _iconStartPosition, _iconEndPosition);
        
        _obstacles.Add(obstacle.ID, (newObstacle, icon));
    }

    public (VisualizedObstacle, ObstacleIcon) CreateObstacle(Obstacle obstacle, float duration)
    {
        Debug.Log($"Добавляю Obstacle {obstacle.ID} {obstacle.Time} {obstacle.BottomState} {obstacle.TopState}");
        
        VisualizedObstacle newObstacle = Instantiate(_obstaclePrefab, _spawnPosition, Quaternion.identity);
        newObstacle.Top.ChangeState(obstacle, true);
        newObstacle.Bottom.ChangeState(obstacle, false);
        
        Alternative(newObstacle, obstacle);
        
        ObstacleIcon icon = Instantiate(_obstacleIconPrefab, _iconStartPosition, Quaternion.identity);
        icon.ChangeIcon(_iconsSO.GetIcon(new DancePair {Top = obstacle.TopState, Bottom = obstacle.BottomState}));
        
        Alternative(icon, obstacle);
        
        _obstacles.Add(obstacle.ID, (newObstacle, icon));
        
        return (newObstacle, icon);
    }

    private void AddVisualization(IObstacleVisualization newObstacle, Obstacle obstacle, Vector2 pos1, Vector2 pos2)
    {
        Debug.Log($"Visualized Obstacle {newObstacle}");

        if (obstacle.Time < _duration)
        {
            newObstacle.Animate(pos1, pos2, _checkCombination.TimeAfter[CheckResultType.BadTime], obstacle.Time);
        }
        else
        {
            newObstacle.Animate(pos1, pos2, _checkCombination.TimeAfter[CheckResultType.BadTime], _duration);
        }
        
        newObstacle.SetID(obstacle.ID);   
        newObstacle.LifetimeEnded += DeleteAndDamage;
    }
    
    private void Alternative(IObstacleVisualization newObstacle, Obstacle obstacle)
    {
        Debug.Log($"Visualized Obstacle {newObstacle}");

        newObstacle.SetID(obstacle.ID);   
        newObstacle.LifetimeEnded += DeleteAndDamage;
    }

    private void DeleteAndDamage(int id)
    {
        Debug.Log("Delete and damage");
        _healthSystem.ChangeHealth(CheckResultType.TooLate);
        DeleteObstacle(id);
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
        obstacleObject.Item1.LifetimeEnded -= DeleteAndDamage;
        obstacleObject.Item2.OnDestroy();
        obstacleObject.Item2.LifetimeEnded -= DeleteAndDamage;
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
        
        obstacleObject.Item1.LifetimeEnded -= DeleteAndDamage;
        obstacleObject.Item1.OnDestroy();
        obstacleObject.Item2.LifetimeEnded -= DeleteAndDamage;
        obstacleObject.Item2.OnDestroy();
    }

    public void DeleteAll()
    {
        foreach (KeyValuePair<int, (IObstacleVisualization, IObstacleVisualization)> obstacle in _obstacles)
        {
            obstacle.Value.Item1.LifetimeEnded -= DeleteAndDamage;
            obstacle.Value.Item1.OnDestroy();
            obstacle.Value.Item2.LifetimeEnded  -= DeleteAndDamage;
            obstacle.Value.Item2.OnDestroy();
        }
        
        _obstacles.Clear();
    }
}