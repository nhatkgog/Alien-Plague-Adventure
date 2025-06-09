using UnityEngine;

public class Enemy1DieState : EnemyState
{
    protected Enemy1 enemy;

    public Enemy1DieState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy1 enemy)
        : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
