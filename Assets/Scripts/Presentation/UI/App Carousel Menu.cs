using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

public class AppCarouselMenu : MonoBehaviour, IPrefLoader, IPrefSaver
{
    [SerializeField] private string _prefKey = "BlockingInfo";

    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Dictionary<RectTransform, App> _icons;
    [SerializeField] private TMP_Text _appName;

    [SerializeField] private float _itemSpacing = 150f;
    [SerializeField] private float _selectedScale = 1.3f;
    [SerializeField] private float _scaleSpeed = 5f;
    [SerializeField] private float _scrollSmoothTime = 0.12f;
    [SerializeField] private float _edgePullDistance = 28f;
    [SerializeField] private float _edgeReturnTime = 0.15f;

    [SerializeField] private string _swapAppSound;
    [SerializeField] private string _appOpenSound;
    [SerializeField] private string _appBlockedSound;
    [SerializeField] private string _gameStartedSound;

    private int _selectedIndex;
    private float _contentVelocity;
    private float _edgeOffset;
    private float _edgeOffsetVelocity;

    private List<KeyValuePair<RectTransform, App>> _orderedIcons;
    private SoundSystem _soundSystem;
    private bool _isLoadingBlockingInfo;

    public string PrefKey => _prefKey;
    public int SelectedIndex => _selectedIndex;

    [Inject]
    private void Construct(SoundSystem soundSystem)
    {
        _soundSystem = soundSystem;
    }

    private void Start()
    {
        EnsureOrderedIcons();

        foreach (KeyValuePair<RectTransform, App> pair in _orderedIcons)
        {
            pair.Value.BlockingStateChanged += OnBlockingStateChanged;
        }

        _soundSystem.PlaySound(_appOpenSound);

        float firstY = (_orderedIcons.Count - 1) * _itemSpacing / 2f;

        for (int i = 0; i < _orderedIcons.Count; i++)
        {
            RectTransform icon = _orderedIcons[i].Key;

            icon.anchorMin = new Vector2(0.5f, 0.5f);
            icon.anchorMax = new Vector2(0.5f, 0.5f);
            icon.pivot = new Vector2(0.5f, 0.5f);
            icon.anchoredPosition = new Vector2(i * _itemSpacing - firstY, 0f);
            icon.localScale = Vector3.one;
        }

        _content.anchorMin = new Vector2(0.5f, 0.5f);
        _content.anchorMax = new Vector2(0.5f, 0.5f);
        _content.pivot = new Vector2(0.5f, 0.5f);
        _content.anchoredPosition = new Vector2(
            firstY,
            _content.anchoredPosition.y
        );

        UpdateSelectedApp();
    }

    private void Update()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll > 0f)
        {
            if (_selectedIndex > 0)
            {
                _selectedIndex--;
                _edgeOffset = 0f;
                _edgeOffsetVelocity = 0f;
                UpdateSelectedApp();
            }
            else
            {
                _edgeOffset = -_edgePullDistance;
                _edgeOffsetVelocity = 0f;
            }
        }
        else if (scroll < 0f)
        {
            if (_selectedIndex < _orderedIcons.Count - 1)
            {
                _selectedIndex++;
                _edgeOffset = 0f;
                _edgeOffsetVelocity = 0f;
                UpdateSelectedApp();
            }
            else
            {
                _edgeOffset = _edgePullDistance;
                _edgeOffsetVelocity = 0f;
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            _menuPanel.SetActive(false);

            if (_orderedIcons[_selectedIndex].Value.IsBlocked == false)
            {
                _orderedIcons[_selectedIndex].Value.Open();
                _soundSystem.PlaySound(_appOpenSound);
            }
            else
            {
                _soundSystem.PlaySound(_appBlockedSound);
            }
        }

        _edgeOffset = Mathf.SmoothDamp(
            _edgeOffset,
            0f,
            ref _edgeOffsetVelocity,
            _edgeReturnTime
        );

        Vector2 contentPosition = _content.anchoredPosition;
        contentPosition.x = Mathf.SmoothDamp(
            contentPosition.x,
            -_orderedIcons[_selectedIndex].Key.anchoredPosition.x + _edgeOffset,
            ref _contentVelocity,
            _scrollSmoothTime
        );
        _content.anchoredPosition = contentPosition;

        foreach (KeyValuePair<RectTransform, App> pair in _orderedIcons)
        {
            float targetScale = pair.Key == _orderedIcons[_selectedIndex].Key
                ? _selectedScale
                : 1f;

            pair.Key.localScale = Vector3.MoveTowards(
                pair.Key.localScale,
                Vector3.one * targetScale,
                _scaleSpeed * Time.unscaledDeltaTime
            );
        }
    }

    public void SetAppBlocked(App app, bool isBlocked)
    {
        if (isBlocked)
        {
            app.Block();
        }
        else
        {
            app.Unblock();
        }
    }

    public string GetSaveData()
    {
        EnsureOrderedIcons();

        return new string(_orderedIcons
            .Select(pair => pair.Value.IsBlocked ? '0' : '1')
            .ToArray());
    }

    public void LoadData(string data)
    {
        EnsureOrderedIcons();

        if (string.IsNullOrEmpty(data))
        {
            return;
        }

        _isLoadingBlockingInfo = true;

        for (int i = 0; i < _orderedIcons.Count && i < data.Length; i++)
        {
            App app = _orderedIcons[i].Value;

            if (data[i] == '1')
            {
                app.Unblock();
            }
            else
            {
                app.Block();
            }
        }

        _isLoadingBlockingInfo = false;
        UpdateSelectedApp(false);
    }

    private void OnBlockingStateChanged()
    {
        if (_isLoadingBlockingInfo)
        {
            return;
        }

        PlayerPrefs.SetString(PrefKey, GetSaveData());
        PlayerPrefs.Save();

        UpdateSelectedApp(false);
    }

    private void EnsureOrderedIcons()
    {
        _orderedIcons ??= _icons
            .OrderBy(pair => pair.Value.AppPriority)
            .ToList();
    }

    private void UpdateSelectedApp(bool playSound = true)
    {
        if (playSound)
        {
            _soundSystem.PlaySound(_swapAppSound);
        }

        App app = _orderedIcons[_selectedIndex].Value;
        _appName.text = app.IsBlocked ? $"{app.Name} [CLOSED]" : app.Name;

        foreach (KeyValuePair<RectTransform, App> pair in _orderedIcons)
        {
            pair.Value.AppIcon.color = pair.Value.IsBlocked
                ? Color.gray * 0.5f
                : Color.white;
        }
    }

    private void OnDestroy()
    {
        if (_orderedIcons == null)
        {
            return;
        }

        foreach (KeyValuePair<RectTransform, App> pair in _orderedIcons)
        {
            pair.Value.BlockingStateChanged -= OnBlockingStateChanged;
        }
    }
}
