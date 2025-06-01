using UnityEngine;

public class Enemy1AttackState : EnemyState
{
    private Enemy1 enemy;
    public Enemy1AttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.SetZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
