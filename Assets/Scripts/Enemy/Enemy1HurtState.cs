using UnityEngine;

public class Enemy1HurtState : EnemyState
{
    protected Enemy1 enemy;
    private float hurtDuration = 0.1f;
    private float timer;

    public Enemy1HurtState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName, Enemy1 enemy)
        : base(enemyBase, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer > hurtDuration)
        {
            stateMachine.ChangeState(enemy.idelState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
