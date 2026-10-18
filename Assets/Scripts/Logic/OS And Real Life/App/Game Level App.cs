using UnityEngine;
using Zenject;

public class GameLevelApp : App
{
    private AudioClip _startSong;
    private SongSO _startSongSO;
    
    private GameEntryPoint _entry;
    
    private ObstacleGeneration _obstacleGeneration;
    
    [Inject]
    private void Construct(GameEntryPoint entry, ObstacleGeneration generation)
    {
        _entry = entry;
        
        _obstacleGeneration = generation;
    }

    public override void Open()
    {
        base.Open();
        _entry.StartGame(_startSong, _startSongSO);
    }

    public override void Close()
    {
        base.Close();
        _obstacleGeneration.Stop();
    }
}
