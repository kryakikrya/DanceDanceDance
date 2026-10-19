using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Zenject;

public class GameSaveLoader
{
    private List<IPrefLoader> _loaders;

    public GameSaveLoader(List<IPrefLoader> loaders)
    {
        _loaders = loaders;
    }
    
    public void LoadAll()
    {
        foreach (IPrefLoader loader in _loaders)
        {
            if (!PlayerPrefs.HasKey(loader.PrefKey))
            {
                continue;
            }

            string data = ((IPrefSaver)loader).GetSaveData();
            loader.LoadData(data);
        }
    }
}