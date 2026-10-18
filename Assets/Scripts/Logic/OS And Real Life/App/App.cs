using UnityEngine;

public class App : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [SerializeField] private GameObject _appPanel;

    public virtual void Open()
    {
        _appPanel.SetActive(true);
    }

    public virtual void Close()
    {
        _appPanel.SetActive(false);
    }
}
