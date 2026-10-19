using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccessoryButton : MonoBehaviour
{
    [SerializeField] private GameObject _firstObject;
    [SerializeField] private GameObject _secondObject;
    [SerializeField] private TMP_Text _stateText;

    private bool _isEnabled = false;

    private void Start()
    {
        UpdateState();
    }

    public void ChangeState()
    {
        _isEnabled = !_isEnabled;
        UpdateState();
    }

    private void UpdateState()
    {
        _firstObject.SetActive(_isEnabled);
        _secondObject.SetActive(_isEnabled);
        _stateText.text = _isEnabled ? "Off" : "On";
    }
}