using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebindService : MonoBehaviour, IPrefSaver, IPrefLoader
{
    [SerializeField] private InputActionReference _actionReference;
    [SerializeField] private string _prefKey;

    public string PrefKey => _prefKey;

    private InputAction Action => _actionReference.action;

    public string GetSaveData()
    {
        return Action.bindings[0].overridePath ?? string.Empty;
    }

    public void LoadData(string data)
    {
        if (string.IsNullOrEmpty(data))
            return;

        Action.ApplyBindingOverride(0, data);
        Debug.Log($"[Rebind] Loaded binding: {Action.name} -> {data}", this);
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

                Debug.Log($"[Rebind] Saved: action='{Action.name}', key='{PrefKey}', path='{data}'", this);
            })
            .OnCancel(operation =>
            {
                Debug.LogWarning($"[Rebind] Cancelled: action='{Action.name}', key='{PrefKey}'", this);

                operation.Dispose();
                Action.Enable();
            })
            .Start();
    }
}