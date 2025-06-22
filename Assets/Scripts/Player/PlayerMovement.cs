using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class InputSystemMovement : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI ammoText;

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
    public float bombThrowHeight = 5f;
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

    private bool isGrounded = false;
    private bool isFacingRight = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fx = GetComponent<EntityFX>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        PlayerStatus data = PlayerSelector.Instance.GetSelectedPlayer();
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

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {

        #region Jump
        // Jump
        PlayerMove();
        PlayerJump();
        PlayerShooting();
        PlayerExplosion();
        PlayerReset();
        PlayerLevelUp();
        PlayerRecharge();
        // PlayerHurt(10f);
        // PlayerDead();
        IsGroundDetected();
        IsWallDetected();

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

    void PlayerExplosion() 
    {
        if (Input.GetKeyDown(KeyCode.K) && currentBoom > 0 && Time.time >= nextBombTime)
        {
            Debug.Log("🔹 Bắt đầu tạo boom...");

            nextBombTime = Time.time + bombCooldown;

            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector3 start = boomFirePoint.position;
                Vector3 end = nearestEnemy.position;

                // Tạo quả boom
                GameObject bomb = Instantiate(bombPrefab, start, Quaternion.identity);

                currentBoom--;
                Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 velocity = CalculateParabolaVelocity(start, end, bombThrowHeight);
                    rb.linearVelocity = velocity;
                }

                // Gọi animation ném (nếu có)
                //animator.SetTrigger("Throw");
            }
            Debug.Log("✅ Boom đã được tạo!");

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

    void PlayerReset()
    {

    }
    void PlayerLevelUp()
    {

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
        bool isSprinting = sprintAction.ReadValue<float>() > 0;
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
                ammoText.text = currentBullet.ToString();
            }
            else
            {
                ammoText.text = "EMPTY";
            }
        }
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        Transform nearest = null;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    Vector2 CalculateParabolaVelocity(Vector3 start, Vector3 end, float height)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float displacementY = end.y - start.y;
        Vector2 displacementX = new Vector2(end.x - start.x, 0f);

        float timeUp = Mathf.Sqrt(2 * height / gravity);
        float timeDown = Mathf.Sqrt(2 * (height - displacementY) / gravity);
        float totalTime = timeUp + timeDown;

        float velocityY = Mathf.Sqrt(2 * gravity * height);
        float velocityX = displacementX.x / totalTime;

        return new Vector2(velocityX, velocityY);
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
}
