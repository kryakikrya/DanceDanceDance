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

   public CheckCombination(SongClock clock, ObstacleGeneration generation, float obstacleTime, float timeBefore, float timeAfter)
   {
      _timeBefore = timeBefore;
      _timeAfter = timeAfter;
      
      _generation = generation;

      _generation.OnPoseChanged += ChangeObstacle;

      _songClock = clock;
   }

   private void ChangeObstacle(Obstacle newObstacle)
   {
      _previousObstacle = _currentObstacle;
      
      _currentObstacle = newObstacle;
   }

   public CheckResultType Check(DanceState topState, DanceState bottomState)
   {
      if (Mathf.Abs(CurrentTime - _previousObstacle.Time) > Mathf.Abs(CurrentTime - _currentObstacle.Time))
      {
         if (CurrentTime < _previousObstacle.Time + _timeAfter)
         {
            if (_previousObstacle.BottomState == bottomState && _previousObstacle.TopState == topState)
            {
               return CheckResultType.PerfectTime;  
            }
            
            return CheckResultType.WrongCombination;
         }

         return CheckResultType.TooLate;
      }

      if (CurrentTime > _currentObstacle.Time - _timeBefore)
      {
         if (_currentObstacle.BottomState == bottomState && _currentObstacle.TopState == topState)
         {
            return CheckResultType.PerfectTime;  
         }
            
         return CheckResultType.WrongCombination;
      }

      return CheckResultType.TooEarly;

      return CheckResultType.WrongCombination;
   }
}