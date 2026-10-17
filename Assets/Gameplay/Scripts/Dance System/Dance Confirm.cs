using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DanceConfirm : MonoBehaviour
{
    [SerializeField] private BodyPart _top;
    [SerializeField] private BodyPart _bottom;
    
    private PlayerActions _playerActions;
    private CheckCombination _checker;
    
    [Inject]
    private void Construct(PlayerActions playerActions, CheckCombination checker)
    {
        _playerActions = playerActions;
        
        _checker = checker;
    }

    private void OnEnable()
    {
        _playerActions.MainMap.Confirm.performed += OnConfirm;
    }

    private void OnDisable()
    {
        _playerActions.MainMap.Confirm.performed -= OnConfirm;
    }
    
    private void OnConfirm(InputAction.CallbackContext context)
    {
        _checker.Check(_top.CurrentState, _bottom.CurrentState);
    }
}
