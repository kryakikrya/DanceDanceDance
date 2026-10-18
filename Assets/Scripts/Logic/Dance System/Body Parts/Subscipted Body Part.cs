using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class SubsciptedBodyPart : BodyPart
{
    [Inject] private PlayerActions _playerActions;
    
    [Header("Bindings")]

    private readonly List<(InputAction Action, Action<InputAction.CallbackContext> Callback)> _subscriptions = new List<(InputAction Action, Action<InputAction.CallbackContext> Callback)>();

    private void OnEnable()
    {
        foreach (var binding in _bindings)
        {
            var reference = binding.Action.action;

            var action = _playerActions.asset.FindAction($"{reference.actionMap.name}/{reference.name}", true);
            
            var state = binding.StateData;

            Action<InputAction.CallbackContext> callback = _ => ChangeState(state);

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
}