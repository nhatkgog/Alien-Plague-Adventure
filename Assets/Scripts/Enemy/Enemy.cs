using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    public EntityFX entityFX;
    public Enemy1 enemy;
    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    public Transform attackCheck;
    public float attckCheckRadius;
    public float attackDamage;

    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    [SerializeField] private Image hpBar;
    public int health = 50;
    private float maxHP;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        maxHP = health;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState?.Update();
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerDetected()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);
    }


    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        stateMachine.ChangeState(enemy.hurtState);
        UpdateHealthBar();

        if (entityFX != null)
            entityFX.PlayHitFX();

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        stateMachine.ChangeState(enemy.dieState);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        Destroy(gameObject, 2f);
    }

    protected void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = (float)health / maxHP;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attckCheckRadius);
    }
}
