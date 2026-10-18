using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] protected StateDataSO _startState;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    
    [SerializeField] protected List<Binding> _bindings;
    
    protected DanceState _currentState;
    
    public DanceState CurrentState => _currentState;
    
    protected CustomAnimator _animator;
    
    private Vector2 _basePosition;

    private void Awake()
    {
        _animator = new CustomAnimator(_spriteRenderer);
        
        _basePosition =  _spriteRenderer.transform.localPosition;
    }
    
    public void ChangeState(DanceState state)
    {
        Debug.Log($"Changing state to {state}");
        
        foreach (var bind in _bindings)
        {
            Debug.Log($"Checking for state {bind.StateData.State}");
            if (bind.StateData.State == state)
            {
                _currentState = state;
                
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
        Debug.Log($"Changing state to {state.State}");
        
        _currentState = state.State;
        _spriteRenderer.transform.localPosition = _basePosition + state.Offset;
        if (_animator == null)
        {
            _animator = new CustomAnimator(_spriteRenderer);
        }
        
        _animator.PlayAnimation(state.Sprites, state.TimeBetweenSprites).Forget();
    }
}
