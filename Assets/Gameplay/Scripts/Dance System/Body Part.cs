using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class BodyPart : MonoBehaviour
{
    [Inject] private PlayerActions _playerActions;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private StateDataSO _startState;
    
    [Header("Bindings")]
    
    [SerializeField] private List<Binding> _bindings;

    private readonly List<(InputAction Action, Action<InputAction.CallbackContext> Callback)> _subscriptions = new List<(InputAction Action, Action<InputAction.CallbackContext> Callback)>();
    
    private DanceState _currentState;
    
    public DanceState CurrentState => _currentState;

    private void OnEnable()
    {
        ChangeState(_startState);
        
        foreach (var binding in _bindings)
        {
            var reference = binding.Action.action;

            var action = _playerActions.asset.FindAction($"{reference.actionMap.name}/{reference.name}", true);
            
            var state = binding.StateData;
            
            Debug.Log(
                $"[INPUT BIND] {name} | Action: {action.actionMap.name}/{action.name} | " +
                $"Bindings: {action.GetBindingDisplayString()} | State: {state}"
            );

            Action<InputAction.CallbackContext> callback = _ => ChangeState(state);
            
            Debug.Log($"Action: {action.name} | Enabled: {action.enabled}");

            action.performed += callback;
            _subscriptions.Add((action, callback));
        }
    }

    private void OnDisable()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Action.performed -= subscription.Callback;
        }

        _subscriptions.Clear();
    }

    private void ChangeState(StateDataSO state)
    {
        Debug.Log($"Changing state to {state}");
        
        _currentState = state.State;

        _spriteRenderer.sprite = state.Sprite;
    }
}