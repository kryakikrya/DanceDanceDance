using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckCombination
{
   private Dictionary<CheckResultType, float> _timeBefore;
   private Dictionary<CheckResultType, float> _timeAfter;
   
   private SongClock _songClock;
   
   private ObstacleGeneration _generation;

   private Obstacle _currentObstacle;
   private Obstacle _previousObstacle;
   
   private float CurrentTime => _songClock.SongTime;

   public event Action<Obstacle> OnObstacleCreated;
   public event Action<Obstacle> OnObstacleDestroyed;
   
   public Dictionary<CheckResultType, float> TimeBefore => _timeBefore;
   public Dictionary<CheckResultType, float> TimeAfter => _timeAfter;
   public CheckCombination(SongClock clock, ObstacleGeneration generation, Dictionary<CheckResultType, float> timeBefore, Dictionary<CheckResultType, float> timeAfter)
   {
      _timeBefore = timeBefore;
      _timeAfter = timeAfter;
      
      Debug.Log($"Checker has been initialized. {_timeBefore} - {_timeAfter}");
      
      _generation = generation;

      _generation.OnObstacleCreated += ChangeObstacle;

      _songClock = clock;
   }

   public void ChangeSettings(Dictionary<CheckResultType, float> before, Dictionary<CheckResultType, float> after)
   {
      Debug.Log($"Changed {before[CheckResultType.PerfectTime]}");
      
      _timeBefore = before;
      
      _timeAfter = after;
   }

   private void ChangeObstacle(Obstacle newObstacle)
   {
      _previousObstacle = _currentObstacle;
      
      _currentObstacle = newObstacle;
      
      OnObstacleCreated?.Invoke(newObstacle);
   }

   public void ChangeObstacleNoSpawn(Obstacle newObstacle)
   {
      _previousObstacle = _currentObstacle;
      
      _currentObstacle = newObstacle;
   }

   private static readonly CheckResultType[] _timeResults =
   {
      CheckResultType.PerfectTime,
      CheckResultType.NiceTime,
      CheckResultType.GoodTime,
      CheckResultType.BadTime
   };

   public CheckResultType Check(DanceState topState, DanceState bottomState)
   {
      Debug.Log($"{CurrentTime} времени сейчас. {_previousObstacle.Time} - prev, {_currentObstacle.Time} - cur");

      Obstacle obstacle;
      Dictionary<CheckResultType, float> timeWindows;
      CheckResultType outOfTimeResult;

      Debug.Log($"{_currentObstacle.Time} {_currentObstacle.BottomState}  {_previousObstacle.Time} {_previousObstacle.BottomState}");
      

      {
         if (Mathf.Abs(CurrentTime - _previousObstacle.Time) <
             Mathf.Abs(CurrentTime - _currentObstacle.Time) && _previousObstacle.Time != 0)
         {
            Debug.Log("PREVIOUS");
            obstacle = _previousObstacle;
            timeWindows = _timeAfter;
            outOfTimeResult = CheckResultType.TooLate;
         }
         else
         {
            Debug.Log("CURRENT");
            obstacle = _currentObstacle;
            timeWindows = _timeBefore;
            outOfTimeResult = CheckResultType.TooEarly;
         }  
      }

      float timeDifference = Mathf.Abs(CurrentTime - obstacle.Time);
      CheckResultType result = outOfTimeResult;

      foreach (CheckResultType timeResult in _timeResults)
      {
         Debug.Log($"Рассматриваю {timeResult}.");
         Debug.Log($"{timeDifference} и {timeWindows[timeResult]}.");
         
         if (timeDifference < timeWindows[timeResult])
         {
            Debug.Log($"Да!");
            result = timeResult;
            break;
         }
      }

      if (result == outOfTimeResult)
      {
         return FinalizeCheck(obstacle, outOfTimeResult);
      }

      if (obstacle.BottomState == bottomState && obstacle.TopState == topState)
      {
         return FinalizeCheck(obstacle, result);
      }

      Debug.Log($"Input {topState} + {bottomState}. It was {obstacle.TopState} - {obstacle.BottomState}");
      return FinalizeCheck(obstacle, CheckResultType.WrongCombination);
   }

   public CheckResultType FinalizeCheck(Obstacle obstacle, CheckResultType type)
   {
      Debug.Log($"Result {type}. {obstacle.ID} {obstacle.Time} {obstacle.TopState} {obstacle.BottomState}");
      OnObstacleDestroyed?.Invoke(obstacle);
      return type;
   }
}