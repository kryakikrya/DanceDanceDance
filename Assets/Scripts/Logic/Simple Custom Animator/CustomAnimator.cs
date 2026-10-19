using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CustomAnimator : IDisposable
{
    private CancellationTokenSource _cts = new CancellationTokenSource();
    
    private SpriteRenderer _spriteRenderer;

    public CustomAnimator(SpriteRenderer spriteRenderer)
    {
        _spriteRenderer = spriteRenderer;
    }
    
    public async UniTaskVoid PlayAnimation(List<Sprite> animation, float timeBetweenSprites)
    {
        Dispose();
        _cts = new CancellationTokenSource();
        
        try
        {
            int index = 0;
            
            while (!_cts.Token.IsCancellationRequested)
            {
                Debug.Log($"Меняю спрайт на {animation[index].name}");
                _spriteRenderer.sprite = animation[index];
                index++;

                if (index >= animation.Count)
                {
                    index = 0;
                }
                
                await UniTask.WaitForSeconds(timeBetweenSprites, cancellationToken: _cts.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Animation Cancelled");
        }
    }
    
    public void Dispose()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();  
            _cts = new CancellationTokenSource();
        }
    }
}