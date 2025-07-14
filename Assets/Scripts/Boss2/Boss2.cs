using System.Collections;
using UnityEngine;

public class Boss2 : SampleEnemy
{

    private bool hasRevived = false;
    private bool hasUsedMeteor = false;

    [Header("Normal Attack")]
    [SerializeField] private GameObject redWolfHitbox;

    private Animator animator;

    [Header("Tornado")]
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private float tornadoSpeed = 5f;
    [SerializeField] private Transform firePoint; // giống IceBall
    [SerializeField] private int bulletCount = 5;

    [Header("Wolf Explosion")]
    [SerializeField] private GameObject wolfPrefab;
    [SerializeField] private Transform wolfSpawnPoint;

    [Header("Meteor")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float meteorSpawnHeight = 10f;

    [Header("Revived")]
    [SerializeField] private GameObject revivedPrefab;
    [SerializeField] private Transform revivedPoint;
    [SerializeField] private Material revivedOutlineMaterial;
    private SpriteRenderer spriteRenderer;
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHp = 1000f;
        currentHp = maxHp;
        UpdateHpBar();

    }
    protected override void Update()
    {
        base.Update();

        IsGroundDetected();

        IsWallDetected();

        MoveToPlayer();

        if (Time.time - lastSkillTime >= skillCooldown)
        {
            UseRandomAttackSkill();
            lastSkillTime = Time.time;
        }
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    WolfExplosion();
        //}
        FlipEnemy();
    }

    public void ComboAttack()
    {
        animator.SetTrigger("Attack");

        float pushDistance = 0.5f;
        Vector3 pushDir = facingRight ? Vector3.right : Vector3.left;
        transform.position += pushDir * pushDistance;
        Debug.Log("Combo attack");
    }

    public void Claw()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float clawRange = 6f;

        if (distance <= clawRange)
        {
            animator.SetTrigger("Claw");

            Vector3 dashDir = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(dashDir.x * 8f, rb.linearVelocity.y);

        }
        Debug.Log("Claw");

    }
    public void Bite()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        float biteRange = 3f;

        if (distance <= biteRange)
        {
            animator.SetTrigger("Bite");
            rb.linearVelocity = Vector2.zero;

        }
        Debug.Log("Bite");

    }

    public void FireTornado()
    {
        if (hasRevived)
        {

            Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;

            float offsetStep = 1.3f;
            int middleIndex = bulletCount / 2;

            for (int i = 0; i < bulletCount; i++)
            {
                float yOffset = (i - middleIndex) * offsetStep;
                Vector3 spawnPos = firePoint.position + new Vector3(0, yOffset, 0);

                // Tạo tornado tại vị trí bắn
                GameObject tornado = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);

                // Thiết lập hướng bay và tốc độ
                Tornado tornadoScript = tornado.GetComponent<Tornado>();
                if (tornadoScript != null)
                {
                    tornadoScript.SetMovementDirection(directionToPlayer, tornadoSpeed);
                }
            }
        }

    }

    public void WolfExplosion()
    {
        //if (player == null || wolfPrefab == null || wolfSpawnPoint == null) return;

        for (int i = 0; i < 2; i++)
        {
            // Tạo vị trí lệch trái và phải
            float offsetX = i == 0 ? -2f : 2f;
            Vector3 spawnPos = wolfSpawnPoint.position + new Vector3(offsetX, 0, 0);

            GameObject wolf = Instantiate(wolfPrefab, spawnPos, Quaternion.identity);

            wolf.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Xoay hướng đúng theo vị trí player
            if (player.transform.position.x < spawnPos.x)
                wolf.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }

        Debug.Log("WolfExplosion summoned!");
    }

    public void Meteor()
    {
        float horizontalOffset = Random.Range(-6f, 6f); // Random trái hoặc phải
        float verticalOffset = Random.Range(4f, 8f);    // Random độ cao

        Vector3 spawnOffset = new Vector3(horizontalOffset, verticalOffset, 0);
        Vector3 spawnPos = player.transform.position + spawnOffset;

        Instantiate(meteorPrefab, spawnPos, Quaternion.identity);
    }

    protected override void Die()
    {
        if (!hasRevived)
        {
            hasRevived = true;
            currentHp = maxHp;
            UpdateHpBar();
            var effect = Instantiate(revivedPrefab, revivedPoint.position, Quaternion.identity);
            Destroy(effect,2f);
            spriteRenderer.material = revivedOutlineMaterial;
            return; // Không Destroy
        }

        base.Die();
    }


    public void EnableFenrirHitbox()
    {
        if (redWolfHitbox != null)
            redWolfHitbox.SetActive(true);
    }

    public void DisableFenrirHitbox()
    {
        if (redWolfHitbox != null)
            redWolfHitbox.SetActive(false);
    }
    private void UseRandomAttackSkill()
    {
        if (!hasRevived)
        {
            // Giai đoạn 1: chỉ dùng kỹ năng vật lý
            int skillIndex = Random.Range(0, 3);
            switch (skillIndex)
            {
                case 0: ComboAttack(); break;
                case 1: Claw(); break;
                case 2: Bite(); break;
            }
        }
        else
        {
            // Giai đoạn 2: dùng thêm kỹ năng phép thuật
            float hpPercent = currentHp / maxHp;

            if (hpPercent <= 0.2f && !hasUsedMeteor)
            {
                Meteor();
                hasUsedMeteor = true;
                return;
            }

            int skillIndex = Random.Range(0, 5);
            switch (skillIndex)
            {
                case 0: ComboAttack(); break;
                case 1: Claw(); break;
                case 2: Bite(); break;
                case 3:
                    animator.SetTrigger("Attack");
                    break;
                case 4: WolfExplosion(); break;
            }
        }
    }


    public override bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, hit ? Color.green : Color.red);
        return hit.collider != null;
    }

    private bool isAttacking = false;

    private IEnumerator EndAttackAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    private void MoveToPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > attackDistance) // Chỉ di chuyển nếu chưa tới khoảng cách đánh
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * enemyMoveSpeed, rb.linearVelocity.y);

            // Lật hướng
            if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
                Flip();
        }
        else
        {
            rb.linearVelocity = Vector2.zero; // Dừng lại khi tới gần
        }
    }

}
