using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [Header("Services")]
    [SerializeField] private SongClock _clock;
    [SerializeField] private CombinationSettingsSO combinationSettings;
    
    [Header("Sound System")]
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    [Header("Player Settings")]
    
    [SerializeField] private int _maxHealth;
    
    [Header("Valid time before and after obstacle")]
    // Аккуратно! Короче если там before и after двух обстаклов друг на друга наложатся, то плохо может быть, так что значения в идеале не слишком большие
    // По идее сработает только Previous, а второй останется, но в любом случае не стоит
    [SerializeField] private float _timeBefore;

    [SerializeField] private float _timeAfter;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void InstallBindings()
    {
        GameEntryPoint gameEntryPoint = new GameEntryPoint(_audioSources, _clock, combinationSettings, _timeBefore, _timeAfter, _maxHealth);
        
        gameEntryPoint.StartServices(
            out CMS contentManagementSystem, 
            out SoundSystem soundSystem,
            out ObstacleGeneration generation,
            out PlayerActions playerActions,
            out ScoreSystem scoreSystem,
            out CheckCombination checkCombination,
            out HealthSystem healthSystem);
        
        Container.Bind<CMS>().FromInstance(contentManagementSystem).AsSingle();
        Container.Bind<SongClock>().FromInstance(_clock).AsSingle();
        Container.Bind<SoundSystem>().FromInstance(soundSystem).AsSingle();
        Container.Bind<ObstacleGeneration>().FromInstance(generation).AsSingle();
        Container.Bind<PlayerActions>().FromInstance(playerActions).AsSingle();
        Container.Bind<ScoreSystem>().FromInstance(scoreSystem).AsSingle();
        Container.Bind<CheckCombination>().FromInstance(checkCombination).AsSingle();
        Container.Bind<HealthSystem>().FromInstance(healthSystem).AsSingle();
        
        gameEntryPoint.StartGame(
            soundSystem, 
            _startSong, 
            _startSongSO, 
            generation, 
            playerActions);
    }
}
