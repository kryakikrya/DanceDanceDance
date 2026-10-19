using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private BodyPart _part;
    
    [SerializeField] private SpriteRenderer _copySpriteRenderer;
    [SerializeField] private List<Binding> _bindings;
    
    private Vector2 _basePosition;

    private void Awake()
    {
        _basePosition  = _copySpriteRenderer.transform.localPosition;
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(SwapItem);
        _copySpriteRenderer.sprite = _part.Sprite;  
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SwapItem);
    }

    public void SwapItem()
    {
        _part.SwapSprites(_bindings);
        _copySpriteRenderer.sprite = _part.Sprite;
        
        _copySpriteRenderer.transform.localPosition =  _basePosition + _bindings[0].StateData.Offset;
        Debug.Log(_bindings[0].StateData.Offset);
        
        Debug.Log($"Swapped item {_part.name}. {_part} {_copySpriteRenderer.sprite}");
    }
}