using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StateDataSO", menuName = "Scriptable Objects/StateDataSO")]
public class StateDataSO : ScriptableObject
{
    [SerializeField] private DanceState state;
    public DanceState State => state;

    [SerializeField]
    private List<Sprite> _sprites;
    public List<Sprite> Sprites => _sprites;
    
    [SerializeField] private float _timeBetweenSprites;
    public float TimeBetweenSprites => _timeBetweenSprites;
    
    [SerializeField] private Vector2 _offset;
    public Vector2 Offset => _offset;
}