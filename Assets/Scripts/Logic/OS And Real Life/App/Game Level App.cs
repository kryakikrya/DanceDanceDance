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
    
    [Inject]
    private void Construct(GameEntryPoint entry, ObstacleGeneration generation, PlayerActions playerActions, ScoreSystem scoreSystem, SoundSystem soundSystem)
    {
        Debug.Log($"Game Level App Construct {entry}");
        
        _entry = entry;
        
        _obstacleGeneration = generation;
        
        _scoreSystem = scoreSystem;
        
        _playerActions = playerActions;
        
        _soundSystem = soundSystem;
    }

    public override void Open()
    {
        base.Open();
        
        Debug.Log($"Open! {_entry}, {_startSongSO.name}, {_startSong}");
        _entry.StartGame(_startSong, _startSongSO);
    }

    public override void Close()
    {
        _obstacleGeneration.Stop();
        _playerActions.Disable();
        _scoreSystem.Reset();
        _soundSystem.StopAllSounds();
        
        base.Close();
    }
}
