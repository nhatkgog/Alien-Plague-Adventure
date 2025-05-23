public class Enemy1IdleState : EnemyState
{
    private Enemy1 enemy;
    public Enemy1IdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy1 _enemy) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.moveState);
    }
}
