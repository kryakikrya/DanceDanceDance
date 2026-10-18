using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameEntryPoint : IDisposable
{
    private PlayerActions _playerActions;
    
    private List<AudioSource> _audioSources;
    
    private SongClock _songClock;
    
    private CombinationSettingsSO _combinationSettingsSo;

    private PlayerActions _actions;
    
    private Dictionary<CheckResultType, float> _timeBefore;
    private Dictionary<CheckResultType, float> _timeAfter;

    private ObstaclesObjectCreator _obstaclesObjectCreator;

    private int _maxHealth;
    
    private int _startHours;
    private int _startMinutes;
    private Image[] _battery;
    
    private  Light2D _light;
    private  Volume _volume;
    
    public GameEntryPoint(List<AudioSource> sources,
        SongClock clock,
        CombinationSettingsSO settings,
        Dictionary<CheckResultType, float> timeBefore,
        Dictionary<CheckResultType, float> timeAfter,
        int maxHealth,
        ObstaclesObjectCreator obstaclesObjectCreator,
        int startHours,
        int startMinutes,
        Image[] battery,
        Light2D light,
        Volume volume)
    {
        _audioSources = sources;
        
        _songClock = clock;
        
        _combinationSettingsSo = settings;
        
        _timeBefore = timeBefore;
        _timeAfter = timeAfter;
        
        _maxHealth = maxHealth;

        _obstaclesObjectCreator = obstaclesObjectCreator;
        
        _startHours = startHours;
        _startMinutes = startMinutes;
        
        _battery = battery;
        
        _light =  light;
        _volume = volume;
    }

    public void StartServices(
        out CMS contentManagementSystem,
        out SoundSystem soundSystem,
        out ObstacleGeneration obstacleGeneration,
        out PlayerActions playerActions,
        out ScoreSystem scoreSystem,
        out CheckCombination checkCombination,
        out HealthSystem healthSystem,
        out ReallifeClock clock,
        out Battery battery
        )
    {
        ResourceLoader loader = new ResourceLoader();
        contentManagementSystem = new CMS(loader);
        soundSystem = new SoundSystem(_audioSources, contentManagementSystem);
        obstacleGeneration = new ObstacleGeneration(_songClock, _obstaclesObjectCreator);
        playerActions = new PlayerActions();
        scoreSystem = new ScoreSystem(_combinationSettingsSo);
        checkCombination = new CheckCombination(_songClock, obstacleGeneration, _timeBefore, _timeAfter);
        healthSystem = new HealthSystem(_maxHealth, _combinationSettingsSo);
        clock = new ReallifeClock(_startHours, _startMinutes);
        battery = new Battery(clock, _battery);
        DayNightChanging dayNightChanging = new DayNightChanging(clock, _light, _volume);
    }

    public void StartGame(
        SoundSystem soundSystem, 
        AudioClip startSong, 
        SongSO startSongSO, 
        ObstacleGeneration generation, 
        PlayerActions playerActions
        )
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