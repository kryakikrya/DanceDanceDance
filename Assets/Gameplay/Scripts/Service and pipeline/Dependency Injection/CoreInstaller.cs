using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller
{
    [Header("Services")]
    [SerializeField] private SongClock _clock;
    
    [Header("Sound System")]
    [SerializeField] private List<AudioSource> _audioSources;
    [SerializeField] private AudioClip _startSong;
    [SerializeField] private SongSO _startSongSO;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void InstallBindings()
    {
        GameEntryPoint gameEntryPoint = new GameEntryPoint(_audioSources, _clock);
        
        gameEntryPoint.StartServices(
            out CMS contentManagementSystem, 
            out SoundSystem soundSystem,
            out ObstacleGeneration generation,
            out PlayerActions playerActions);
        
        Container.Bind<CMS>().FromInstance(contentManagementSystem).AsSingle();
        Container.Bind<SongClock>().FromInstance(_clock).AsSingle();
        Container.Bind<SoundSystem>().FromInstance(soundSystem).AsSingle();
        Container.Bind<ObstacleGeneration>().FromInstance(generation).AsSingle();
        Container.Bind<PlayerActions>().FromInstance(playerActions).AsSingle();
        
        gameEntryPoint.StartGame(
            soundSystem, 
            _startSong, 
            _startSongSO, 
            generation, 
            playerActions);
    }
}
