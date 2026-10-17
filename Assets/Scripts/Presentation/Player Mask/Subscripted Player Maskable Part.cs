using UnityEngine;

public class SubscriptedPlayerMaskablePart : PlayerMaskablePart
{
    [SerializeField] private PlayerMaskablePart _pair;

    public override void ChangeState(Obstacle state, bool isTop)
    {
        base.ChangeState(state, isTop);
        _pair.ChangeState(state, isTop);
    }
}
