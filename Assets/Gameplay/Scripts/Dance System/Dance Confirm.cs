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
    private ScoreSystem _scoreSystem;
    
    [Inject]
    private void Construct(PlayerActions playerActions, CheckCombination checker, ScoreSystem scoreSystem)
    {
        _playerActions = playerActions;
        
        _checker = checker;
        
        _scoreSystem = scoreSystem;
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
        Debug.Log($"Confirm top {_top.gameObject.name} is {_top.CurrentState}, bot {_bottom.gameObject.name} is {_bottom.CurrentState}");
        
        _scoreSystem.AddScore(_checker.Check(_top.CurrentState, _bottom.CurrentState));
    }
}
