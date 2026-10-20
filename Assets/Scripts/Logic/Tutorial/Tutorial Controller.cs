using System;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ObstaclesObjectCreator _creator;
    [SerializeField] private TutorialStep[] _steps;

    [SerializeField] private Vector2 _spawnPosition;
    [SerializeField] private Vector2 _playerPosition;
    [SerializeField] private Vector2 _spawnPositionIcon;
    [SerializeField] private Vector2 _endPositionIcon;
    [SerializeField] private float _duration;
    
    [SerializeField] private InputActionReference[] _actions;

    private CheckCombination _combinations;
    private SongClock _songClock;

    [Inject]
    private void Construct(CheckCombination combinations, SongClock songClock)
    {
        _combinations = combinations;
        
        _songClock = songClock;
    }

    public async void Tutorial()
    {
        _text.text = "";
        _text.gameObject.SetActive(true);
        _songClock.SetIsPlaying(false);
        
        await RunTutorial();
        
        _songClock.SetIsPlaying(true);
    }

    private async UniTask RunTutorial()
    {
        foreach (TutorialStep step in _steps)
        {
            _combinations.ChangeObstacleNoSpawn(step.Obstacle);
            
            var (visualizedObstacle, obstacleIcon) =
                _creator.CreateObstacle(step.Obstacle, step.Duration);

            obstacleIcon.AnimationAsync(_endPositionIcon, _duration).Forget();
            await visualizedObstacle.AnimationAsync(_spawnPosition, _playerPosition, _duration);

            _text.gameObject.SetActive(true);
            _text.text = step.Text;
            
            UpdateBindingTexts();

            if (visualizedObstacle != null)
            {
                await WaitForObstacleDestroyed();                
            }

            _text.gameObject.SetActive(false);
        }
        
        _text.gameObject.SetActive(false);
    }

    private async UniTask WaitForObstacleDestroyed()
    {
        UniTaskCompletionSource completionSource = new();

        void OnObstacleDestroyed(Obstacle obstacle)
        {
            completionSource.TrySetResult();
        }

        _combinations.OnObstacleDestroyed += OnObstacleDestroyed;

        try
        {
            await completionSource.Task;
        }
        finally
        {
            _combinations.OnObstacleDestroyed -= OnObstacleDestroyed;
        }
    }
    
    private void UpdateBindingTexts()
    {
        foreach (InputActionReference actionReference in _actions)
        {
            string displayName = Regex.Replace(
                actionReference.action.bindings[0].effectivePath,
                @"^<[^>]+>/",
                "");

            displayName = Regex.Replace(displayName, @"([a-z])([A-Z])", "$1 $2");

            if (displayName.Length > 0)
            {
                displayName = char.ToUpper(displayName[0]) + displayName.Substring(1);
            }

            _text.text = _text.text.Replace(
                $"{{{actionReference.action.name}}}",
                displayName);
        }
    }

    [Serializable]
    private class TutorialStep
    {
        [SerializeField] private DanceState _top;
        [SerializeField] private DanceState _bottom;
        [SerializeField] private float _duration;
        [SerializeField] private string _text;

        public Obstacle Obstacle => new Obstacle(0, _top, _bottom, 0.01f);
        public float Duration => _duration;
        public string Text => _text;
    }
}