using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class SaveSystemInstaller : MonoInstaller
{
    [SerializeField] private List<GameObject> _objects;

    [SerializeField] private bool _deleteAll;
        
    public override void InstallBindings()
    {
        if (_deleteAll)
        {
            PlayerPrefs.DeleteAll();
        }
        
        List<IPrefLoader> prefLoaders = new List<IPrefLoader>();
        
        foreach (var gameObject in _objects)
        {
            prefLoaders.Add(gameObject.GetComponent<IPrefLoader>());
        }
        
        Container.Bind<List<IPrefLoader>>().FromInstance(prefLoaders).AsSingle();
        
        GameSaveLoader loader = new GameSaveLoader(prefLoaders);
        loader.LoadAll();
        
        Container.Bind<Money>().AsSingle();
    }
}
