using Assets.Scripts.Save_and_Load;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSystemMovement : MonoBehaviour, ISaveManager
{
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private TextMeshProUGUI boomText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private Image expBar;

    [SerializeField] private float chargePower = 0f;
    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;
    [SerializeField] private float baseThrowSpeed = 6f;
    private bool isChargingBoom = false;
    private int chargeDirection = 1; // 1: tăng, -1: giảm

    [SerializeField] private Image chargeBar;
    [SerializeField] private GameObject charging;

    //ground
    public Transform groundCheck;
    public float groundCheckDistance;
    public Transform wallCheck;
    public float wallCheckDistance;
    public LayerMask whatIsGround;
    public int facingDir { get; private set; } = 1;

    public EntityFX fx { get; private set; }

    //Player Status
    private float speed;
    private float maxHealth;
    private float def;
    private float endurance;
    private float exp;
    private int level;
    private float money;

    //bullet
    public GameObject bulletPrefab;
    public Transform firePoint;
    private int maxBullet;
    public static float damage;
    private float shootDelay;

    //boom
    public GameObject bombPrefab;
    public Transform boomFirePoint;
    //public float bombThrowHeight = 5f;
    public float bombCooldown = 2f;
    private float nextBombTime;
    public static float damageBoom;
    public static float explosionRadius;
    private int maxBoomQuatity;
    public static float knockbackForce;

    //reduce
    private float currentHealth;
    private int currentBullet;
    private int currentExp;
    private int currentBoom;
    private float nextshoot;

    public float jumpForce;
    public float sprintMultiplier;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;
    private PlayerStatus data;

    private bool isGrounded = false;
    private bool isFacingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        data = PlayerSelector.Instance.GetSelectedPlayer();
        if (data != null)
        {
            if (animator != null)
                animator.runtimeAnimatorController = data.animatorController;
            speed = data.moveSpeed;
            maxHealth = data.maxHealth;
            def = data.def;
            endurance = data.endurance;

            exp = data.exp;
            level = data.level;

            money = data.money;

            //bullet
            maxBullet = data.maxBulletQuantity;
            damage = data.damage;
            shootDelay = data.shootDelay;
            //boom
            damageBoom = data.damageBoom;
            explosionRadius = data.explosionRadius;
            maxBoomQuatity = data.maxBoomQuatity;
            knockbackForce = data.knockbackForce;

        }
        else
        {
            Debug.LogError("Không có dữ liệu nhân vật được chọn!");
        }
        currentBullet = maxBullet;
        currentHealth = maxHealth;
        currentBoom = maxBoomQuatity;

        updateHPBar();
        updateAmmoText();
        updateBoomText();
        UpdateLevelExpUI();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

        #region Jump
        PlayerMove();
        PlayerJump();
        PlayerShooting();
        PlayerRecharge();
        IsGroundDetected();
        IsWallDetected();

        HandleBoomCharging();
        UpdateLevelExpUI();
        FixedUpdate();
        #endregion
    }

    void PlayerMove()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        // Flip sprite
        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }
    void PlayerJump()
    {
        if (jumpAction.triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }
    void PlayerShooting()
    {
        if (Input.GetKeyDown(KeyCode.J) && currentBullet > 0 && Time.time > nextshoot)
        {
            nextshoot = Time.time + shootDelay;
            animator.SetTrigger("Shooting");

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Ignore collision between player and bullet
            Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();

            currentBullet--;
            updateAmmoText();
            if (bulletCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(bulletCollider, playerCollider);
            }
        }
    }

    void PlayerDead()
    {
        rb.isKinematic = true; // Disable physics
        rb.linearVelocity = Vector2.zero; // Stop movement
        animator.SetTrigger("Dead");
        Invoke(nameof(DestroyPlayer), 2f); // Delay before destroying the player object
    }
    public void DestroyPlayer()
    {
        Destroy(gameObject);
    }
    void PlayerRecharge()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentBullet < maxBullet)
        {
            animator.SetTrigger("Recharge");
            currentBullet = maxBullet;
            updateAmmoText();
        }

    }
    public void PlayerHurt(float takeDamage)
    {
        currentHealth -= takeDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hurt");
            updateHPBar();
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            PlayerDead();
        }

    }

    void FixedUpdate()
    {
        float enduranceRecoveryRate = 2f; // tốc độ hồi
        float enduranceDrainRate = 8f;   // tốc độ tụt
        float minEnduranceToSprint = 1f;  // cần ít nhất 1 để chạy nhanh

        bool isSprinting = sprintAction.ReadValue<float>() > 0 && endurance > minEnduranceToSprint;
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        if (isSprinting)
        {
            animator.SetFloat("Speed", 1);
        }
        else
        {
            animator.SetFloat("Speed", 0.5f);
        }
        if (moveInput.x == 0)
        {
            animator.SetFloat("Speed", 0);
        }

        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);

        if (isSprinting && moveInput.x != 0)
        {
            endurance -= enduranceDrainRate * Time.fixedDeltaTime;
            endurance = Mathf.Max(0f, endurance);
        }
        else
        {
            endurance += enduranceRecoveryRate * Time.fixedDeltaTime;
            endurance = Mathf.Min(data.endurance, endurance);
        }
        //charging.gameObject.SetActive(true);

        //chargeBar.fillAmount = endurance / data.endurance;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    void updateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHealth / maxHealth;
        }
    }
    private void updateAmmoText()
    {
        if (ammoText != null)
        {
            if (currentBullet > 0)
            {
                ammoText.text = "Bullets: " + currentBullet.ToString();
            }
            else
            {
                ammoText.text = "Bullets: EMPTY";
            }
        }
    }
    private void updateBoomText()
    {
        if (boomText != null)
        {
            if (currentBoom > 0)
            {
                boomText.text = "Booms: " + currentBoom.ToString();
            }
            else
            {
                boomText.text = "Booms: EMPTY";
            }
        }
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        updateHPBar();
    }

    #region Collision
    public virtual bool IsGroundDetected()
        => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected()
        => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    public virtual void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance, 0));
        if (wallCheck != null)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y, 0));
    }
    #endregion

    void HandleBoomCharging()
    {
        if (Input.GetKeyDown(KeyCode.K) && currentBoom > 0 && Time.time >= nextBombTime)
        {
            isChargingBoom = true;
            chargePower = 0f;
            chargeDirection = 1;

            charging.gameObject.SetActive(true);
        }

        if (Input.GetKey(KeyCode.K) && isChargingBoom)
        {
            chargePower += chargeSpeed * Time.deltaTime * chargeDirection;

            if (chargePower >= maxCharge)
            {
                chargePower = maxCharge;
                chargeDirection = -1;
            }
            else if (chargePower <= 0f)
            {
                chargePower = 0f;
                chargeDirection = 1;
            }

            chargeBar.fillAmount = chargePower / maxCharge;

        }

        if (Input.GetKeyUp(KeyCode.K) && isChargingBoom)
        {
            isChargingBoom = false;
            nextBombTime = Time.time + bombCooldown;
            charging.gameObject.SetActive(false);
            ThrowBoom();
        }
    }
    void ThrowBoom()
    {
        Vector3 start = boomFirePoint.position;
        GameObject bomb = Instantiate(bombPrefab, start, Quaternion.identity);
        currentBoom--;
        updateBoomText();

        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float direction = isFacingRight ? 1f : -1f;
            Vector2 force = new Vector2(direction * chargePower * baseThrowSpeed, chargePower * baseThrowSpeed);
            rb.linearVelocity = force;
        }

        //animator.SetTrigger("Throw");
    }

    public void LoadData(GameData _data)
    {
        currentHealth = _data.health > 0 ? _data.health : currentHealth;
        currentBullet = _data.bulletCount > 0 ? _data.bulletCount : currentBullet;
        currentBoom = _data.boomCount > 0 ? _data.boomCount : currentBoom;
        money = _data.currency > 0 ? _data.currency : money;

        updateHPBar();
    }

    public void SaveData(GameData _data)
    {
        _data.health = currentHealth;
        _data.bulletCount = currentBullet;
        _data.boomCount = currentBoom;
        _data.currency = money;
    }
    void UpdateLevelExpUI()
    {
        float currentExp = data.exp;
        float expNeeded = Mathf.Pow(10, data.level);

        if (levelText != null)
            levelText.text = "Level " + data.level;

        if (expText != null)
            expText.text = Mathf.RoundToInt(data.exp) + " / " + expNeeded;

        if (expBar != null)
        {
            float targetFill = currentExp / expNeeded;
            expBar.fillAmount = Mathf.Lerp(expBar.fillAmount, targetFill, Time.deltaTime * 10f);
        }
    }
    public void HealToFull()
    {
        currentHealth = maxHealth;
        updateHPBar();
    }


}
