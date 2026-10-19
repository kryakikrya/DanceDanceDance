using System;
using UnityEngine;

public class AutouseOffset : MonoBehaviour
{
    [SerializeField]  private SpriteRenderer _spriteRenderer;
    [SerializeField] private StateDataSO _so;

    private void Start()
    {
        _spriteRenderer.transform.localPosition = _so.Offset;
    }
}
