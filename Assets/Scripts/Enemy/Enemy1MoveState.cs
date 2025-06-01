public class Enemy1MoveState : Enemy1GroundedState
{
    public Enemy1MoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
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
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);

        if (enemy.IsWallDetected() || enemy.IsGroundDetected())
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.idelState);
        }
    }
}
