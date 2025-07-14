using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boss1 : SampleEnemy
{
    [Header("Ice Ball")]
    [SerializeField] private GameObject iceBallPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedBullet = 15f;
    [SerializeField] private int bulletCount = 5;
    private Animator animator;

    [Header("Frozen Breath")]
    [SerializeField] private GameObject frozenBreathPrefab;
    [SerializeField] private Transform breathPoint;
    [SerializeField] private AnimationClip frozenBreathClip;


    [Header("Cryo Drop")]
    [SerializeField] private GameObject iceDropPrefab;
    [SerializeField] private float spreadWidth = 5f;
    [SerializeField] private int dropCount = 10;
    [SerializeField] private float dropInterval = 0.2f; // thời gian giữa mỗi lần rơi

    [Header("Ragnarok Awakening")]
    [SerializeField] private float scaleMultiplier = 1.5f;
    [SerializeField] private float maxHpMultiplier = 2f;
    [SerializeField] private float skillDamageMultiplier = 2f;
    public static float damageBonus = 1f;
    private bool hasAwakened = false;


    [Header("Fenrir's Devour")]
    [SerializeField] private GameObject fenrirHitbox;

    [SerializeField] private float jumpForce = 17f;

    [SerializeField] private Material awakingOutlineMaterial;
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

        if (Time.time - lastSkillTime >= skillCooldown)
        {
            UseRandomAttackSkill();
            lastSkillTime = Time.time;
        }

        if (!hasAwakened && currentHp / maxHp < 0.2f)
        {
            RagnarokAwakening();
        }

        float hpPercent = player.currentHealth / player.maxHealth;
        if (hpPercent < 0.2f)
        {
            FenrirDevour();
        }
        FlipEnemy();
    }


    public void IceBall()
    {
        Vector3 directionToPlayer = (player.transform.position - firePoint.position).normalized;

        float offsetStep = 1.3f;
        int middleIndex = bulletCount / 2;

        for (int i = 0; i < bulletCount; i++)
        {
            float yOffset = (i - middleIndex) * offsetStep;
            Vector3 spawnPos = firePoint.position + new Vector3(0, yOffset, 0);

            GameObject bullet = Instantiate(iceBallPrefabs, spawnPos, Quaternion.identity);
            IceBullet enemyBullet = bullet.AddComponent<IceBullet>();
            enemyBullet.SetMovementDirection(directionToPlayer, speedBullet);
        }
    }



    public void FrozenBreath()
    {
        float animDuration = frozenBreathClip != null ? frozenBreathClip.length : 1.5f;

        GameObject breath = Instantiate(frozenBreathPrefab, breathPoint.position, Quaternion.identity);
        FrozenBreath effect = breath.GetComponent<FrozenBreath>();
        if (effect != null)
        {
            // Nếu boss đang quay trái → đảo hướng
            if (transform.localScale.x < 0)
            {
                breath.transform.localScale = new Vector3(-breath.transform.localScale.x, breath.transform.localScale.y, 1);
                breath.transform.right = -transform.right;
            }
            else
            {
                breath.transform.right = transform.right;
            }

            effect.Initialize(animDuration);
        }

    }


    public void CryoDrop()
    {
        if (player == null) return;
        StartCoroutine(SpawnCryoDrops());
    }

    private IEnumerator SpawnCryoDrops()
    {
        float spawnY = player.transform.position.y + 7f;
        float centerX = player.transform.position.x;

        for (int i = 0; i < dropCount; i++)
        {
            float randomX = centerX + Random.Range(-spreadWidth / 2f, spreadWidth / 2f);
            float randomYOffset = Random.Range(0f, 1.5f);
            Vector3 spawnPos = new Vector3(randomX, spawnY + randomYOffset, 0);

            Instantiate(iceDropPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(dropInterval);
        }
    }
    public void StartCryoDrop()
    {

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical velocity
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        animator.SetTrigger("IsJump");

        Debug.Log("Boss bắt đầu nhảy lên.");

    }

    public void RagnarokAwakening()
    {
        if (hasAwakened) return; 
        hasAwakened = true;
        // Nhân đôi máu tối đa và hồi đầy máu
        maxHp *= maxHpMultiplier;
        currentHp = maxHp;
        UpdateHpBar();
        spriteRenderer.material = awakingOutlineMaterial;

        // Tăng kích thước boss
        transform.localScale *= scaleMultiplier;

        // Tăng sát thương kỹ năng
        damageBonus *= (int)skillDamageMultiplier;

        Debug.Log("Boss has awakened: Ragnarok mode!");
    }

    private bool hasDevoured = false;

    public void FenrirDevour()
    {
        if (hasDevoured) return;

        if (player != null)
        {
            hasDevoured = true;
            float hpPercent = player.currentHealth / player.maxHealth;

            if (hpPercent < 0.2f)
            {
                // Dịch chuyển tới vị trí người chơi
                transform.position = player.transform.position;

                // Kích hoạt animation
                animator.SetTrigger("IsRunActtack");

                Debug.Log("Fenrir Devour activated! Player executed.");
            }
        }
    }

    // Kích hoạt và tắt hitbox từ animation event
    public void EnableFenrirHitbox()
    {
        if (fenrirHitbox != null)
            fenrirHitbox.SetActive(true);
    }

    public void DisableFenrirHitbox()
    {
        if (fenrirHitbox != null)
            fenrirHitbox.SetActive(false);
    }

    private void UseRandomAttackSkill()
    {
        int skillIndex = Random.Range(0, 3); // 0: IceBall, 1: FrozenBreath, 2: CryoDrop

        switch (skillIndex)
        {
            case 0:
                animator.SetTrigger("IsAttack");
                break;
            case 1:
                FrozenBreath();
                break;
            case 2:
                StartCryoDrop(); // animation Jump nằm trong đó
                break;
        }
    }

    public override bool IsGroundDetected()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, hit ? Color.green : Color.red);
        return hit.collider != null;
    }
}
