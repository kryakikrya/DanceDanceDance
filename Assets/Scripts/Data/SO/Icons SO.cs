using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IconsSO", menuName = "Scriptable Objects/IconsSO")]
public class IconsSO : ScriptableObject
{ 
    [SerializeField] private Dictionary<DancePair, Sprite> _icons = new Dictionary<DancePair, Sprite>();
    
    public Sprite GetIcon(DancePair pair)
    {
        foreach (var icon in _icons)
        {
            if (icon.Key.Top == pair.Top && icon.Key.Bottom == pair.Bottom)
            {
                return icon.Value;
            }
        }
        
        return null;
    }
}

[System.Serializable]
public struct DancePair
{
  public DanceState Top;
  public DanceState Bottom;
}
