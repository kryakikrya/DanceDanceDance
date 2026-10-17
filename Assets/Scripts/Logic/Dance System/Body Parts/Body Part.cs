using UnityEngine;

public class BodyPart : MonoBehaviour
{
    [SerializeField] protected StateDataSO _startState;
    
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    
    protected DanceState _currentState;
    
    public DanceState CurrentState => _currentState;

    public virtual void ChangeState(StateDataSO state)
    {
        Debug.Log($"Changing state to {state.State}");
        
        _currentState = state.State;

        _spriteRenderer.sprite = state.Sprite;
    }
}
