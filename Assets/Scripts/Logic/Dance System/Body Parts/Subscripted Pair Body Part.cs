using UnityEngine;

public class SubscriptedPairBodyPart : SubsciptedBodyPart
{
    [SerializeField] private BodyPart _pair;

    public override void ChangeState(StateDataSO state)
    {
        base.ChangeState(state);
        
        _pair.ChangeState(state.State);
    }
}
