using System;
using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Image AppIcon { get; private set; }

    [field: SerializeField] public int AppPriority { get; private set; }

    [SerializeField] private GameObject _appPanel;

    [SerializeField] protected bool _isBlocked;

    public bool IsBlocked => _isBlocked;

    public virtual void Open()
    {
        _appPanel.SetActive(true);
    }

    public void Unblock()
    {
        _isBlocked = false;
    }

    public void Block()
    {
        _isBlocked = true;
    }

public virtual void Close()
    {
        _appPanel.SetActive(false);
    }
}
