using UnityEngine;

[CreateAssetMenu(fileName = "StateDataSO", menuName = "Scriptable Objects/StateDataSO")]
public class StateDataSO : ScriptableObject
{
    [SerializeField] private DanceState state;
    public DanceState State => state;
    
    [SerializeField] private Sprite _sprite;
    public Sprite Sprite => _sprite;
}