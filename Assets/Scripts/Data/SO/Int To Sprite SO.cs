using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntToSpriteSO", menuName = "Scriptable Objects/IntToSpriteSO")]
public class IntToSpriteSO : ScriptableObject
{
    [SerializeField] private Dictionary<int, Sprite> _intToSprite = new Dictionary<int, Sprite>();
    
    public Dictionary<int, Sprite> IntToSprite => _intToSprite;
}
