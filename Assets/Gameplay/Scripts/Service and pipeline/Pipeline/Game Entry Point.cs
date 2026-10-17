using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GameEntryPoint : IDisposable
{
    private PlayerActions _playerActions;
    
    private List<AudioSource> _audioSources;
    
    private SongClock _songClock;
    
    private CombinationSettingsSO _combinationSettingsSo;

    private PlayerActions _actions;
    
    private float _timeBefore;
    private float _timeAfter;

    private int _maxHealth;
    
    public GameEntryPoint(List<AudioSource> sources, SongClock clock, CombinationSettingsSO settings, float timeBefore, float timeAfter, int maxHealth)
    {
        _audioSources = sources;
        
        _songClock = clock;
        
        _combinationSettingsSo = settings;
        
        _timeBefore = timeBefore;
        _timeAfter = timeAfter;
        
        _maxHealth = maxHealth;
    }

    public void StartServices(
        out CMS contentManagementSystem,
        out SoundSystem soundSystem,
        out ObstacleGeneration obstacleGeneration,
        out PlayerActions playerActions,
        out ScoreSystem scoreSystem,
        out CheckCombination checkCombination,
        out HealthSystem healthSystem
        )
    {
        ResourceLoader loader = new ResourceLoader();
        contentManagementSystem = new CMS(loader);
        soundSystem = new SoundSystem(_audioSources, contentManagementSystem);
        obstacleGeneration = new ObstacleGeneration(_songClock);
        playerActions = new PlayerActions();
        scoreSystem = new ScoreSystem(_combinationSettingsSo);
        checkCombination = new CheckCombination(_songClock, obstacleGeneration, _timeBefore, _timeAfter);
        healthSystem = new HealthSystem(_maxHealth, _combinationSettingsSo);
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