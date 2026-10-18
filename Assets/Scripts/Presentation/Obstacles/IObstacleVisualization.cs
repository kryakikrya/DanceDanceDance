using System;
using UnityEngine;

public interface IObstacleVisualization
{
    public event Action<int> LifetimeEnded;
    
    public void SetID(int id);

    public void Animate(Vector2 spawnPosition, Vector2 playerPosition, float timeAfter, float duration);

    public void OnDestroy();
}
