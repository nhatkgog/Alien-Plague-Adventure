public class Enemy1 : Enemy
{
    #region States
    public Enemy1IdleState idelState { get; private set; }
    public Enemy1MoveState moveState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idelState = new Enemy1IdleState(this, stateMachine, "idle", this);
        moveState = new Enemy1MoveState(this, stateMachine, "move", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idelState);
    }

    protected override void Update()
    {
        base.Update();
    }
}
