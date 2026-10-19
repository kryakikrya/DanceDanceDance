using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class SaveSystemInstaller : MonoInstaller
{
    [SerializeField] private List<GameObject> _objects;

    [SerializeField] private bool _deleteAll;
    
    private List<IPrefLoader> _prefLoaders = new List<IPrefLoader>();

        
    public override void InstallBindings()
    {
        if (_deleteAll)
        {
            PlayerPrefs.DeleteAll();
        }
        
        foreach (var gameObject in _objects)
        {
            _prefLoaders.Add(gameObject.GetComponent<IPrefLoader>());
        }
        
        Container.Bind<List<IPrefLoader>>().FromInstance(_prefLoaders).AsSingle();
        
        Container.Bind<Money>().AsSingle();
    }

    private void Awake()
    {
        GameSaveLoader loader = new GameSaveLoader(_prefLoaders);
        loader.LoadAll();
    }
}
