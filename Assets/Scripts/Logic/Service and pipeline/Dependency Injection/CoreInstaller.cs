using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [Header("Services")]
    [SerializeField] private SongClock _clock;
    [SerializeField] private CombinationSettingsSO _combinationSettings;
    [SerializeField] private ObstaclesObjectCreator  _obstaclesObjectCreator;
    
    [Header("Sound System")]
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    [Header("Player Settings")]
    
    [SerializeField] private int _maxHealth;
    
    [Header("Valid time before and after obstacle")]
    // Аккуратно! Короче если там before и after двух обстаклов друг на друга наложатся, то плохо может быть, так что значения в идеале не слишком большие
    // По идее сработает только Previous, а второй останется, но в любом случае не стоит
    [SerializeField] private Dictionary<CheckResultType, float> _typeToBeforeTime = new Dictionary<CheckResultType, float>();
    public Dictionary<CheckResultType, float> BeforeTime => _typeToBeforeTime;
    
    [SerializeField] private Dictionary<CheckResultType, float> _typeToAfterTime = new Dictionary<CheckResultType, float>();
    public Dictionary<CheckResultType, float> AfterTime => _typeToAfterTime;

    [Header("Battery and Time")]
    
    [SerializeField] private int _startHours;
    [SerializeField] private int _startMinutes;
    [SerializeField] private Image[] _battery;
    
    [Header("Day and Night System")]
    
    [SerializeField] private  Light2D _light;
    [SerializeField] private  Volume _volume;
    
    private GameEntryPoint _gameEntryPoint;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void InstallBindings()
    {
        _gameEntryPoint = 
            new GameEntryPoint(
                _audioSources, 
                _clock, 
                _combinationSettings, 
                _typeToBeforeTime, 
                _typeToAfterTime, 
                _maxHealth, 
                _obstaclesObjectCreator,
                _startHours,
                _startMinutes,
                _battery,
                _light,
                _volume);
        
        _gameEntryPoint.StartServices(
            out CMS contentManagementSystem, 
            out SoundSystem soundSystem,
            out ObstacleGeneration generation,
            out PlayerActions playerActions,
            out ScoreSystem scoreSystem,
            out CheckCombination checkCombination,
            out HealthSystem healthSystem,
            out ReallifeClock clock,
            out Battery battery);
        
        Container.Bind<CMS>().FromInstance(contentManagementSystem).AsSingle();
        Container.Bind<SongClock>().FromInstance(_clock).AsSingle();
        Container.Bind<SoundSystem>().FromInstance(soundSystem).AsSingle();
        Container.Bind<ObstacleGeneration>().FromInstance(generation).AsSingle();
        Container.Bind<PlayerActions>().FromInstance(playerActions).AsSingle();
        Container.Bind<ScoreSystem>().FromInstance(scoreSystem).AsSingle();
        Container.Bind<CheckCombination>().FromInstance(checkCombination).AsSingle();
        Container.Bind<HealthSystem>().FromInstance(healthSystem).AsSingle();
        Container.Bind<ReallifeClock>().FromInstance(clock).AsSingle();
        Container.Bind<Battery>().FromInstance(battery).AsSingle();
    }
}
