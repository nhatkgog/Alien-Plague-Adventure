using UnityEngine;

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
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine { get; private set; }

    public int health = 50;
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState?.Update();
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 50, whatIsPlayer);

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

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

        // Disable Collider so player & bullets can pass through
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        // Stop Rigidbody movement and make it non-interactive
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true; // Stop physics simulation
        }

        // Finally, destroy after a delay (to let animation finish)
        GameObject.Destroy(this.gameObject, 2f);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }
}
