using UnityEngine;
using UnityEngine.UI;

public class App : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Image AppIcon { get; private set; }

    [field: SerializeField] public int AppPriority { get; private set; }

    [SerializeField] private GameObject _appPanel;
    
    [SerializeField] private GameObject _menuPanel;

    [SerializeField] protected bool _isBlocked;

    [SerializeField] protected Button _closeButton;
    public bool IsBlocked => _isBlocked;

    public virtual void Open()
    {
        _appPanel.SetActive(true);
        _closeButton.onClick.AddListener(Close);
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
        _menuPanel.SetActive(true);
        _closeButton.onClick.RemoveAllListeners();
    }

    private void OnDestroy()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}
