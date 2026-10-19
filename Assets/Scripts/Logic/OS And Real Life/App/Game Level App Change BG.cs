using UnityEngine;

public class GameLevelAppChangeBG : GameLevelApp
{
    [SerializeField] private Sprite _newSprite;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    private Sprite _oldSprite;

    public override void Open()
    {
        base.Open();
        
        _oldSprite = _spriteRenderer.sprite;
        _spriteRenderer.sprite = _newSprite;
    }

    public override void Close()
    {
        base.Close();
        
        _spriteRenderer.sprite = _oldSprite;
    }
}
