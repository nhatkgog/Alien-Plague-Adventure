using UnityEngine;

public class Enemy1BattleState : EnemyState
{
    private Transform player;
    private Enemy1 enemy;
    private int moverDir;
    public Enemy1BattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy1 _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = GameObject.Find("Player").transform;
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if (CanAttack())
                    stateMachine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idelState);
        }


        if (player.position.x > enemy.transform.position.x)
            moverDir = 1;
        else if (player.position.x < enemy.transform.position.x)
            moverDir = -1;

        enemy.SetVelocity(enemy.moveSpeed * moverDir, rb.linearVelocity.y);
    }
    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
