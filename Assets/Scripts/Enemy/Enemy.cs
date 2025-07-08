using TMPro;
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

    [Header("SFX")]
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip deathClip;


    public EnemyStateMachine stateMachine { get; private set; }

    [SerializeField] public GameObject coinPrefab;

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

        if (hurtClip != null)
            SFXManager.Instance.PlayOneShot(hurtClip);

        if (health <= 0)
        {
            Die();
            float expEnemy = Random.Range(10,101);
            PlayerSelector.Instance.SetLevelUp(expEnemy);
        }
    }
    protected virtual void Die()
    {
        stateMachine.ChangeState(enemy.dieState);

        if (TryGetComponent(out Collider2D col)) col.enabled = false;
        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (deathClip != null)
            SFXManager.Instance.PlayOneShot(deathClip);

        if (coinPrefab != null)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            TMP_Text missionText = GameObject.Find("CoinValue")?.GetComponent<TMP_Text>();
            Coin coinScript = coin.GetComponent<Coin>();

            if (coinScript != null)
            {
                coinScript.missionCoinText = missionText;
                Debug.Log($"Coin prefab assigned with text: {missionText?.name}");
            }
            else
            {
                Debug.LogWarning("Coin script not found on coin prefab!");
            }
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
