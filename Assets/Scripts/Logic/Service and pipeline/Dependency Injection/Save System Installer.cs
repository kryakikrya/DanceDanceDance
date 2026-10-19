using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveSystemInstaller : MonoInstaller
{
    [SerializeField] private List<GameObject> _objects;
        
    public override void InstallBindings()
    {
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
