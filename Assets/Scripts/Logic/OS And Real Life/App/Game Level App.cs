using UnityEngine;
using Zenject;

public class GameLevelApp : App
{
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    private GameEntryPoint _entry;
    
    private ObstacleGeneration _obstacleGeneration;
    
    private PlayerActions _playerActions;
    
    [Inject]
    private void Construct(GameEntryPoint entry, ObstacleGeneration generation, PlayerActions playerActions)
    {
        Debug.Log($"Game Level App Construct {entry}");
        
        _entry = entry;
        
        _obstacleGeneration = generation;
        
        _playerActions = playerActions;
    }

    public override void Open()
    {
        base.Open();
        
        Debug.Log($"Open! {_entry}, {_startSongSO.name}, {_startSong}");
        _entry.StartGame(_startSong, _startSongSO);
    }

    public override void Close()
    {
        base.Close();
        _obstacleGeneration.Stop();
        _playerActions.Disable();
    }
}
