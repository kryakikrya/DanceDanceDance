using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : IDisposable
{
    private PlayerActions _playerActions;
    
    private List<AudioSource> _audioSources;
    
    private SongClock _songClock;
    
    private ScoreSystemSettingsSO _scoreSystemSettingsSo;

    private PlayerActions _actions;
    
    private float _timeBefore;
    private float _timeAfter;
    
    public GameEntryPoint(List<AudioSource> sources, SongClock clock, ScoreSystemSettingsSO settings, float timeBefore, float timeAfter)
    {
        _audioSources = sources;
        
        _songClock = clock;
        
        _scoreSystemSettingsSo = settings;
        
        _timeBefore = timeBefore;
        _timeAfter = timeAfter;
    }

    public void StartServices(
        out CMS contentManagementSystem,
        out SoundSystem soundSystem,
        out ObstacleGeneration obstacleGeneration,
        out PlayerActions playerActions,
        out ScoreSystem scoreSystem,
        out CheckCombination checkCombination
        )
    {
        ResourceLoader loader = new ResourceLoader();
        contentManagementSystem = new CMS(loader);
        soundSystem = new SoundSystem(_audioSources, contentManagementSystem);
        obstacleGeneration = new ObstacleGeneration(_songClock);
        playerActions = new PlayerActions();
        scoreSystem = new ScoreSystem(_scoreSystemSettingsSo);
        checkCombination = new CheckCombination(_songClock, obstacleGeneration, _timeBefore, _timeAfter);
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
        _playerActions.Dispose();
    }
}