using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameLevelApp : App
{
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    [SerializeField] private SongSO _startSongSOHard;
    
    [SerializeField] private ObstaclesObjectCreator _obstaclesObjectCreator;

    [SerializeField] private float _easyTime = 6.5f;
    [SerializeField] private float _hardTime = 4f;
    
    [SerializeField] private Dictionary<CheckResultType, float> _typeToBeforeTimeEasy = new Dictionary<CheckResultType, float>();
    [SerializeField] private Dictionary<CheckResultType, float> _typeToAfterTimeEasy = new Dictionary<CheckResultType, float>();
    [SerializeField] private Dictionary<CheckResultType, float> _typeToBeforeTimeHard = new Dictionary<CheckResultType, float>();
    [SerializeField] private Dictionary<CheckResultType, float> _typeToAfterTimeHard = new Dictionary<CheckResultType, float>();
    
    private GameEntryPoint _entry;
    
    private ObstacleGeneration _obstacleGeneration;
    
    private PlayerActions _playerActions;
    
    private ScoreSystem _scoreSystem;
    
    private SoundSystem _soundSystem;
    
    private HealthSystem _healthSystem;
    
    private bool _isHard = false;
    
    private CheckCombination _checkCombination;
    
    [Inject]
    private void Construct(GameEntryPoint entry, ObstacleGeneration generation, PlayerActions playerActions, ScoreSystem scoreSystem, SoundSystem soundSystem, HealthSystem system, CheckCombination checkCombination)
    {
        Debug.Log($"Game Level App Construct {entry}");
        
        _entry = entry;
        
        _obstacleGeneration = generation;
        
        _scoreSystem = scoreSystem;
        
        _playerActions = playerActions;
        
        _soundSystem = soundSystem;
        
        _healthSystem = system;
        
        _checkCombination = checkCombination;
    }

    public override void Open()
    {
        base.Open();

        if (_isHard)
        {
            Debug.Log($"Open! {_entry}, {_startSongSOHard.name}, {_startSong}");
            _obstaclesObjectCreator.ChangeFlyTime(_hardTime);
            _entry.StartGame(_startSong, _startSongSOHard);
            _checkCombination.ChangeSettings(_typeToBeforeTimeHard, _typeToAfterTimeHard);
            _checkCombination.ChangeSettings(_typeToBeforeTimeEasy, _typeToAfterTimeEasy);
        }
        else
        {
            Debug.Log($"Open! {_entry}, {_startSongSO.name}, {_startSong}");
            _obstaclesObjectCreator.ChangeFlyTime(_easyTime);
            _entry.StartGame(_startSong, _startSongSO);   
            _checkCombination.ChangeSettings(_typeToBeforeTimeEasy, _typeToAfterTimeEasy);
        }
        
        _healthSystem.OnPlayerDead += Close;
    }

    public void ChangeDifficulty(bool difficulty)
    {
        _isHard = difficulty;
    }

    public override void Close()
    {
        Debug.Log($"Close! {_entry}, {_startSongSO.name}, {_startSong}");
        
        base.Close();
        
        _obstacleGeneration.Stop();
        _playerActions.Disable();
        _scoreSystem.Reset();
        _soundSystem.StopAllSounds();
        
        _healthSystem.OnPlayerDead -= Close;
    }
}
