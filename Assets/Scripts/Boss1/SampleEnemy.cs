using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SampleEnemy : Entity
{
    [Header("Base Stats")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;

    [Header("Movement & Detection")]
    [SerializeField] protected float enemyMoveSpeed = 2f;
    [SerializeField] protected float detectionRange = 3f;
    protected bool isPlayerInRange = false;
    protected InputSystemMovement player;

    [Header("Attack Info")]
    [SerializeField] protected float attackDistance = 3f;
    [SerializeField] protected float skillCooldown = 5f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float lastSkillTime = 0f;
    public Transform attackCheck;

    [Header("UI & Effects")]
    [SerializeField] private Image hpBar;
    [SerializeField] public GameObject coinPrefab;
    [SerializeField] public EntityFX entityFX;

    private float lastAttackTime;

    protected override void Awake()
    {
        base.Awake();
        currentHp = maxHp;
    }

    protected override void Start()
    {
        base.Start();
        player = FindAnyObjectByType<InputSystemMovement>();

        UpdateHpBar();
    }

    protected override void Update()
    {
        base.Update();


    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            float dirToPlayer = player.transform.position.x - transform.position.x;

            if ((dirToPlayer < 0 && facingRight) || (dirToPlayer > 0 && !facingRight))
            {
                Flip();
            }
        }
    }


    //protected void TryAttackPlayer()
    //{
    //    if (Time.time - lastAttackTime >= attackCooldown)
    //    {
    //        float distance = Vector2.Distance(transform.position, player.transform.position);
    //        if (distance <= attackDistance)
    //        {
    //            lastAttackTime = Time.time;
    //            player.PlayerHurt(attackDamage);
    //        }
    //    }
    //}

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (entityFX != null)
            entityFX.PlayHitFX();

        if (currentHp <= 0)
        {
            Die();
            float expGained = Random.Range(100f, 1001f);
            PlayerSelector.Instance.SetLevelUp(expGained);
        }
    }

    protected virtual void Die()
    {
        if (TryGetComponent(out Collider2D col)) col.enabled = false;
        //rb.linearVelocity = Vector2.zero;
        //rb.isKinematic = true;

        if (coinPrefab != null)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

            TMP_Text missionText = GameObject.Find("CoinValue")?.GetComponent<TMP_Text>();
            Coin coinScript = coin.GetComponent<Coin>();

            if (coinScript != null && missionText != null)
            {
                coinScript.missionCoinText = missionText;
            }
        }

        Destroy(gameObject, 2f);
    }

    public void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackDistance);
    }
}
