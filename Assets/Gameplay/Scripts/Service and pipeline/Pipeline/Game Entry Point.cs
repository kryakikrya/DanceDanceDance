using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : IDisposable
{
    private PlayerActions _playerActions;
    
    private List<AudioSource> _audioSources;
    
    private SongClock _songClock;
    
    public GameEntryPoint(List<AudioSource> sources, SongClock clock)
    {
        _audioSources = sources;
        
        _songClock = clock;
    }

    public void StartServices(
        out CMS contentManagementSystem,
        out SoundSystem soundSystem,
        out ObstacleGeneration obstacleGeneration,
        out PlayerActions playerActions
        )
    {
        ResourceLoader loader = new ResourceLoader();
        contentManagementSystem = new CMS(loader);
        soundSystem = new SoundSystem(_audioSources, contentManagementSystem);
        obstacleGeneration = new ObstacleGeneration(_songClock);
        playerActions = new PlayerActions();
    }

    public void StartGame(
        SoundSystem soundSystem, 
        AudioClip startSong, 
        SongSO startSongSO, 
        ObstacleGeneration generation, 
        PlayerActions playerActions)
    {
        Debug.Log("Starting game");
        
        _songClock.SetIsPlaying(true);
        
        playerActions.Enable();
        
        generation.ChangeSong(startSongSO);
        generation.PlaySong();
        
        soundSystem.PlayMusic(startSong).Forget();
    }

    public void Dispose()
    {
        _audioSources = null;
    }
}