public class Enemy1MoveState : EnemyState
{
    private Enemy1 enemy;
    public Enemy1MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(2 * enemy.facingDir, enemy.rb.linearVelocity.y);

        if (enemy.IsWallDetected() || enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idelState);
        }
    }
}
