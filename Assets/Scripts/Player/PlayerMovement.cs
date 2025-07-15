using Assets.Scripts.Save_and_Load;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Audio;
using System.Threading;
using System;

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
    [SerializeField] private Image enduranceBar;

    [SerializeField] private float chargePower = 0f;
    [SerializeField] private float maxCharge = 2f;
    [SerializeField] private float chargeSpeed = 1f;
    [SerializeField] private float baseThrowSpeed = 6f;
    private bool isChargingBoom = false;
    private int chargeDirection = 1; // 1: tăng, -1: giảm

    [SerializeField] private Image chargeBar;
    [SerializeField] private GameObject charging;
    [SerializeField] private GameOverManager gameManager;

// <<<<<<< HEAD
//     [SerializeField] private AudioClip deathClip;
//     [SerializeField] private AudioClip hurtClip;
//     [SerializeField] private AudioClip reloadClip;
//     [SerializeField] private AudioClip shotClip;
//     [SerializeField] private AudioClip walkClip;
//     [SerializeField] private AudioClip runningClip;
// =======
    [Header("SFX")]
    [SerializeField] private AudioClip deathClip; 
    [SerializeField] private AudioClip hurtClip; 
    [SerializeField] private AudioClip reloadClip; 
    [SerializeField] private AudioClip shotClip; 
    [SerializeField] private AudioClip walkClip; 
    [SerializeField] private AudioClip runningClip;
    [SerializeField] private AudioClip sniffingClip;
    private AudioSource audioSource;

    //ground
    public Transform groundCheck;
    public float groundCheckDistance;
    public Transform wallCheck;
    public float wallCheckDistance;
    public LayerMask whatIsGround;
    public int facingDir { get; private set; } = 1;

    public EntityFX fx { get; private set; }

    //Player Status
    public float speed;
    public float maxHealth;
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
    public float bombThrowHeight = 5f;
    public float bombCooldown = 2f;
    private float nextBombTime;
    public static float damageBoom;
    public static float explosionRadius;
    private int maxBoomQuatity;
    public static float knockbackForce;

    //reduce
    public float currentHealth;
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
            bulletPrefab = data.bulletPrefab;
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
        if (GameStateManager.Instance.IsGameOver() || GameStateManager.Instance.IsVictory() || GameStateManager.Instance.IsPaused())
            return;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Inventory.instance.UseHeal();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Inventory.instance.UseExpItem();
        }
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
        isGrounded = IsGroundDetected();

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
            SFXManager.Instance.PlayOneShot(shotClip);
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

    public bool isDead = false;

    public bool PlayerDead()
    {
        //if (isDead) return true; // prevent multiple calls

        isDead = true;
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Dead");
        Debug.Log("kill");

        SFXManager.Instance.PlayOneShot(deathClip);
        Invoke(nameof(DestroyPlayer), 2f);
        GetComponent<PlayerItemDrop>().GenerateDrop();
        gameManager.ShowGameOver();
        return true;
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
            SFXManager.Instance.PlayOneShot(reloadClip);
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
            SFXManager.Instance.PlayOneShot(hurtClip);
            updateHPBar();
        }
        else if (currentHealth <= 0)
        {
            currentHealth = 0;
            updateHPBar();

            PlayerDead();
        }

    }
    private Boolean wantsToSprint = false;
    void FixedUpdate()
    {
        float enduranceRecoveryRate = 2f;
        float enduranceDrainRate = 8f;

        // Xử lý phím
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Chỉ cho bắt đầu sprint khi endurance đầy
            if (endurance >= data.endurance * 0.99f)
            {
                wantsToSprint = true;
            }
            else
            {
                wantsToSprint = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            wantsToSprint = false;
        }

        // Kiểm tra điều kiện sprint
        bool isMoving = moveInput.x != 0;
        bool isSprinting = wantsToSprint && isMoving && endurance > 0f;

        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        // Animation & SFX
        if (isMoving)
        {
            animator.SetFloat("Speed", isSprinting ? 1f : 0.5f);

            if (isSprinting)
                SFXManager.Instance.PlayLoop(runningClip);
            else
                SFXManager.Instance.PlayLoop(walkClip);
        }
        else
        {
            animator.SetFloat("Speed", 0);
            SFXManager.Instance.StopLoop();
        }

        // Di chuyển
        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);

        // Tính endurance
        if (isSprinting)
        {
            endurance -= enduranceDrainRate * Time.fixedDeltaTime;
            endurance = Mathf.Max(0f, endurance);

            //Nếu endurance cạn → dừng sprint cho tới khi hồi đầy và nhấn lại
            if (endurance <= 0f)
            {
                wantsToSprint = false;
            }
        }
        else
        {
            endurance += enduranceRecoveryRate * Time.fixedDeltaTime;
            endurance = Mathf.Min(data.endurance, endurance);
        }

        if (enduranceBar != null)
            enduranceBar.fillAmount = endurance / data.endurance;

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
        if (!string.IsNullOrEmpty(_data.selectedCharacterName))
        {
            PlayerSelector.Instance.SetSelectedPlayerByName(_data.selectedCharacterName);
            data = PlayerSelector.Instance.GetSelectedPlayer();
        }
        currentHealth = _data.health > 0 ? _data.health : currentHealth;
        currentBullet = _data.bulletCount > 0 ? _data.bulletCount : currentBullet;
        currentBoom = _data.boomCount > 0 ? _data.boomCount : currentBoom;
        money = _data.currency > 0 ? _data.currency : money;

        data.level = _data.level;
        data.exp = _data.exp;
        updateHPBar();
    }

    public void SaveData(GameData _data)
    {
        _data.health = currentHealth;
        _data.bulletCount = currentBullet;
        _data.boomCount = currentBoom;
        _data.currency = money;
        _data.level = data.level;
        _data.exp = data.exp;
        _data.selectedCharacterName = PlayerSelector.Instance.selectedPlayerOriginal.characterName;


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
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        updateHPBar();
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public float GetCurrentExp()
    {
        return data.exp;
    }

    public float GetExpToLevelUp()
    {
        return Mathf.Pow(10, data.level);
    }

    public void AddExp(float amount)
    {
        data.exp += amount;

        float expNeeded = GetExpToLevelUp();
        while (data.exp >= expNeeded)
        {
            data.exp -= expNeeded;
            data.level++;
            Debug.Log($"🎉 Leveled up! New level: {data.level}");
            expNeeded = GetExpToLevelUp();
        }

        UpdateLevelExpUI();
    }

    public void AddBoom()
    {
        currentBoom++;
        updateBoomText();
    }


}
