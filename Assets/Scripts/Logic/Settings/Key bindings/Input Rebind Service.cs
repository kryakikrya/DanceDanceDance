using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputRebindService : MonoBehaviour, IPrefSaver, IPrefLoader
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private string _prefKey;
    [SerializeField] private TMP_Text _bindingText;
    [SerializeField] private TMP_Text _secondaryBindingText;

    public string PrefKey => _prefKey;

    private InputAction Action => _playerActions.asset.FindAction(_actionReference.action.id.ToString());
    
    private PlayerActions _playerActions;

    [Inject]
    private void Construct(PlayerActions playerActions)
    {
        _playerActions = playerActions;
    }

    private void Awake()
    {
        UpdateBindingTexts();
    }

    public string GetSaveData()
    {
        return Action.bindings[0].effectivePath;
    }

    public void LoadData(string data)
    {
        if (string.IsNullOrEmpty(data))
        {
            Action.RemoveBindingOverride(0);
        }
        else
        {
            Action.ApplyBindingOverride(0, data);
        }

        UpdateBindingTexts();
    }

    public void StartRebinding()
    {
        Debug.Log($"[Rebind] Started rebinding: {Action.name}", this);

        Action.Disable();

        Action.PerformInteractiveRebinding(0)
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                Action.Enable();

                string data = GetSaveData();

                PlayerPrefs.SetString(PrefKey, data);
                PlayerPrefs.Save();

                UpdateBindingTexts();

                Debug.Log($"[Rebind] Saved: action='{Action.name}', key='{PrefKey}', path='{data}'", this);
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                Action.Enable();

                UpdateBindingTexts();

                Debug.LogWarning($"[Rebind] Cancelled: action='{Action.name}', key='{PrefKey}'", this);
            })
            .Start();
    }

    private void UpdateBindingTexts()
    {
        string displayName = Regex.Replace(
            Action.bindings[0].effectivePath,
            @"^<[^>]+>/",
            "");

        displayName = Regex.Replace(displayName, @"([a-z])([A-Z])", "$1 $2");

        if (displayName.Length > 0)
        {
            displayName = char.ToUpper(displayName[0]) + displayName.Substring(1);
        }

        _bindingText.text = displayName;
        _secondaryBindingText.text = displayName;
    }
}