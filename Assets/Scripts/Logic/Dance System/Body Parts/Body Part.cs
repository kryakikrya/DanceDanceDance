using System;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] protected StateDataSO _startState;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    
    [SerializeField] protected List<Binding> _bindings;
    
    protected DanceState _currentState;
    
    public DanceState CurrentState => _currentState;
    
    public Sprite Sprite => _spriteRenderer.sprite;
    
    protected CustomAnimator _animator;
    
    private Vector2 _basePosition;


    private void Awake()
    {
        if (_basePosition == Vector2.zero)
        {
            Debug.Log($"{_spriteRenderer.transform.localPosition}");
            _basePosition = _spriteRenderer.transform.localPosition;
        }
    }

    private void OnEnable()
    {
        _animator = new CustomAnimator(_spriteRenderer);
        
        ChangeState(_startState);
    }

    private void OnDisable()
    {
        _animator = null;
        
        ChangeState(_startState);
    }

    public void ChangeState(DanceState state)
    {
        Debug.Log($"Changing state to {state}");
        
        foreach (var bind in _bindings)
        {
            if (bind.StateData.State == state)
            {
                _currentState = state;
                
                Debug.Log($"Swap to {bind.StateData.Sprites[0].name}");
                
                if (_animator == null)
                {
                    _animator = new CustomAnimator(_spriteRenderer);
                }
                
                _animator.PlayAnimation(bind.StateData.Sprites, bind.StateData.TimeBetweenSprites).Forget();

                return;
            }
        }
    }

    public virtual void ChangeState(StateDataSO state)
    {
        Debug.Log($"Changing state to {state.State}. {state.name} {state.Sprites[0]}");
        
        _currentState = state.State;
        
        Debug.Log($"Changing state to {_basePosition} {state.Offset}");
        _spriteRenderer.transform.localPosition = _basePosition + state.Offset;
        
        if (_animator == null)
        {
            _animator = new CustomAnimator(_spriteRenderer);
        }
        
        _animator.PlayAnimation(state.Sprites, state.TimeBetweenSprites).Forget();
    }
    
    public void SwapSprites(List<Binding> bindings)
    {
        Debug.Log($"Swaping sprites for {bindings.Count} bindings");

        if (_basePosition == Vector2.zero)
        {
            Debug.Log($"{_spriteRenderer.transform.localPosition}");
            _basePosition = _spriteRenderer.transform.localPosition;
        }
        
        _bindings = bindings;
        
        _startState = _bindings[0].StateData;
        
        ChangeState(_startState);
    }
}
