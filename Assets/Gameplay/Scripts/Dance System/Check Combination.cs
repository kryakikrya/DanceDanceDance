using System;
using UnityEngine;

public class CheckCombination
{
   private float _timeBefore;
   private float _timeAfter;
   
   private SongClock _songClock;
   
   private ObstacleGeneration _generation;

   private Obstacle _currentObstacle;
   private Obstacle _previousObstacle;
   
   private float CurrentTime => _songClock.SongTime;

   public event Action<Obstacle> OnObstacleCreated;
   public event Action<Obstacle> OnObstacleDestroyed;

   public CheckCombination(SongClock clock, ObstacleGeneration generation, float timeBefore, float timeAfter)
   {
      _timeBefore = timeBefore;
      _timeAfter = timeAfter;
      
      Debug.Log($"Checker has been initialized. {_timeBefore} - {_timeAfter}");
      
      _generation = generation;

      _generation.OnPoseChanged += ChangeObstacle;

      _songClock = clock;
   }

   private void ChangeObstacle(Obstacle newObstacle)
   {
      _previousObstacle = _currentObstacle;
      
      _currentObstacle = newObstacle;
      
      OnObstacleCreated?.Invoke(newObstacle);
   }

   public CheckResultType Check(DanceState topState, DanceState bottomState)
   {
      Debug.Log($"{CurrentTime} времени сейчас. {_previousObstacle.Time} - prev, {_currentObstacle.Time} - cur");
      
      if (Mathf.Abs(CurrentTime - _previousObstacle.Time) < Mathf.Abs(CurrentTime - _currentObstacle.Time))
      {
         if (CurrentTime < _previousObstacle.Time + _timeAfter)
         {
            if (_previousObstacle.BottomState == bottomState && _previousObstacle.TopState == topState)
            {
               return FinalizeCheck(_previousObstacle,  CheckResultType.PerfectTime);
            }
            
            Debug.Log($"Input {topState} + {bottomState}. It was {_previousObstacle.TopState} - {_previousObstacle.BottomState}");
            return FinalizeCheck(_previousObstacle,  CheckResultType.WrongCombination);
         }

         return FinalizeCheck(_previousObstacle,  CheckResultType.TooLate);
      }
      else
      {
         if (CurrentTime > _currentObstacle.Time - _timeBefore)
         {
            if (_currentObstacle.BottomState == bottomState && _currentObstacle.TopState == topState)
            {
               return FinalizeCheck(_currentObstacle,  CheckResultType.PerfectTime);
            }
            
            Debug.Log($"Input {topState} + {bottomState}. It was {_currentObstacle.TopState} - {_currentObstacle.BottomState}");
            return FinalizeCheck(_currentObstacle,  CheckResultType.WrongCombination);
         }

         return FinalizeCheck(_currentObstacle,  CheckResultType.TooEarly);
      }
   }

   public CheckResultType FinalizeCheck(Obstacle obstacle, CheckResultType type)
   {
      OnObstacleDestroyed?.Invoke(obstacle);
      return type;
   }
}