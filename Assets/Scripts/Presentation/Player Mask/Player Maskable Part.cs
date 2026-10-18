using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaskablePart : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] protected SpriteMask _spriteMask;
    
    [SerializeField] private List<StateDataSO> _stateData;

    private Vector2 _basePosition;
    
    private void Awake()
    {
        _basePosition = _spriteMask.transform.localPosition;    
    }

    public virtual void ChangeState(Obstacle obstacle, bool isTop)
    {
        Sprite sprite = null;
        
        foreach (var state in _stateData)
        {
            if (isTop)
            {
                if (state.State == obstacle.TopState)
                {
                    _spriteRenderer.transform.localPosition = _basePosition + state.Offset;
                    sprite = state.Sprites[0];
                }   
            }
            else
            {
                if (state.State == obstacle.BottomState)
                {
                    _spriteRenderer.transform.localPosition = _basePosition + state.Offset;
                    sprite = state.Sprites[0];
                }   
            }
        }

        if (sprite != null)
        {
            _spriteRenderer.sprite = sprite;
            _spriteMask.sprite = sprite;
        }
        else
        {
            Debug.LogWarning($"State data not found");
        }
    }
}
