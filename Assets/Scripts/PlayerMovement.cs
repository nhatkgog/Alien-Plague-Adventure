using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InputSystemMovement : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    public GameObject bulletPrefab;
    public Transform firePoint;
    [SerializeField] private Image hpBar;
    [SerializeField] private TextMeshProUGUI ammoText;

    //Player Status
    private float speed;
    private float maxHealth;
    private int maxBullet;
    public static float damage;
    private float exp;
    private int level;
    private float shootDelay;

    //reduce
    private float currentHealth;
    private int currentBullet;
    private int currentExp;
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
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        PlayerStatus data = PlayerSelector.Instance.GetSelectedPlayer();
        if (data != null)
        {
            if (animator != null)
                animator.runtimeAnimatorController = data.animatorController;
            speed = data.moveSpeed;
            maxHealth = data.maxHealth;
            maxBullet = data.maxBulletQuantity;
            damage = data.damage;
            exp = data.exp;
            level = data.level;
            shootDelay = data.shootDelay;
        }
        else
        {
            Debug.LogError("Không có dữ liệu nhân vật được chọn!");
        }
        currentBullet = maxBullet;
        currentHealth = maxHealth;
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
        PlayerReset();
        PlayerLevelUp();
        PlayerRecharge();
        //PlayerHurt();
        //PlayerDead();

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
        if (Input.GetKeyDown(KeyCode.J)&&currentBullet>0&&Time.time > nextshoot)
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
        animator.SetTrigger("Dead");
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
    void PlayerHurt(float takeDamage)
    {
        currentHealth -= takeDamage;
        currentHealth = Mathf.Max(currentHealth, 0);

        animator.SetTrigger("Hurt");

        updateHPBar();
        if (currentHealth < 0) 
        {
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
}
