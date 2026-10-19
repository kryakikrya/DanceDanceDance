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
    
    [SerializeField] private FakeLoading _fakeLoading;

    [SerializeField] private App _app;
    
    public bool IsBlocked => _isBlocked;
    
    public event System.Action BlockingStateChanged;

    public virtual void Open()
    {
        _appPanel.SetActive(true);
        _closeButton.onClick.AddListener(Close);
        
        _fakeLoading.StartCoroutine(_fakeLoading.PlayLoading());
    }
    
    public void Unblock()
    {
        if (_isBlocked == false)
        {
            return;
        }

        _isBlocked = false;
        BlockingStateChanged?.Invoke();
    }

    public void Block()
    {
        if (_isBlocked)
        {
            return;
        }

        _isBlocked = true;
        BlockingStateChanged?.Invoke();
    }

    public virtual void Close()
    {
        Debug.Log($"App closed {_appPanel} {_menuPanel} {_closeButton}");
        _appPanel.SetActive(false);
        _menuPanel.SetActive(true);
        _closeButton.onClick.RemoveAllListeners();

        _fakeLoading.StartCoroutine(_fakeLoading.PlayLoading());

        if (_app != null)
        {
            _app.Unblock();   
        }
    }

    private void OnDestroy()
    {
        if (_closeButton != null)
        {
            _closeButton.onClick.RemoveAllListeners();
        }
    }
}
