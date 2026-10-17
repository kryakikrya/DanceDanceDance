using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] protected StateDataSO _startState;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    
    protected DanceState _currentState;
    
    public DanceState CurrentState => _currentState;
    
    private CustomAnimator _animator;

    private void Awake()
    {
        _animator = new CustomAnimator(_spriteRenderer);
    }

    public virtual void ChangeState(StateDataSO state)
    {
        Debug.Log($"Changing state to {state.State}");
        
        _currentState = state.State;

        if (_animator == null)
        {
            _animator = new CustomAnimator(_spriteRenderer);
        }
        
        _animator.PlayAnimation(state.Sprites, state.TimeBetweenSprites).Forget();
    }
}
