public struct Obstacle
{
    public readonly int ID;
    public readonly DanceState TopState;
    public readonly DanceState BottomState;
    public readonly float Time;

    public Obstacle(int id, DanceState topState, DanceState bottomState, float time)
    {
        ID = id;
        TopState = topState;
        BottomState = bottomState;
        Time = time;
    }
    
    public (DanceState top, DanceState bottom, float time) Data => (TopState, BottomState, Time); 
}
