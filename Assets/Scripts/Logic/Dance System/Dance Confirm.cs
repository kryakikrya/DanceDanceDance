using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class DanceConfirm : MonoBehaviour
{
    [SerializeField] private SubsciptedBodyPart _top;
    [SerializeField] private SubsciptedBodyPart _bottom;
    
    [SerializeField] private CharacterEmotionController _emotions;
    
    private PlayerActions _playerActions;
    private CheckCombination _checker;
    private ScoreSystem _scoreSystem;
    
    private HealthSystem _healthSystem;
    
    [Inject]
    private void Construct(PlayerActions playerActions, CheckCombination checker, ScoreSystem scoreSystem, HealthSystem healthSystem)
    {
        _playerActions = playerActions;
        
        _checker = checker;
        
        _scoreSystem = scoreSystem;
        
        _healthSystem = healthSystem;
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
        CheckResultType resultType = _checker.Check(_top.CurrentState, _bottom.CurrentState);
        
        Debug.Log($"Confirm {resultType}");
        
        _scoreSystem.AddScore(resultType);
        _healthSystem.ChangeHealth(resultType);
        _emotions.PlayEmotion(resultType);
    }
}
