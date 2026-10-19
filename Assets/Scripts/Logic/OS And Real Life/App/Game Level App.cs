using UnityEngine;
using Zenject;

public class GameLevelApp : App
{
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    private GameEntryPoint _entry;
    
    private ObstacleGeneration _obstacleGeneration;
    
    private PlayerActions _playerActions;
    
    private ScoreSystem _scoreSystem;
    
    private SoundSystem _soundSystem;
    
    private HealthSystem _healthSystem;
    
    [Inject]
    private void Construct(GameEntryPoint entry, ObstacleGeneration generation, PlayerActions playerActions, ScoreSystem scoreSystem, SoundSystem soundSystem, HealthSystem system)
    {
        Debug.Log($"Game Level App Construct {entry}");
        
        _entry = entry;
        
        _obstacleGeneration = generation;
        
        _scoreSystem = scoreSystem;
        
        _playerActions = playerActions;
        
        _soundSystem = soundSystem;
        
        _healthSystem = system;
    }

    public override void Open()
    {
        base.Open();
        
        Debug.Log($"Open! {_entry}, {_startSongSO.name}, {_startSong}");
        _entry.StartGame(_startSong, _startSongSO);
        
        _healthSystem.OnPlayerDead += Close;
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
