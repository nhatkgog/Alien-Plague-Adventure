using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemMovement : MonoBehaviour
{
    private InputAction moveAction;
    private InputAction sprintAction;
    private InputAction jumpAction;

    public int speed;
    public float jumpForce;
    public int sprintMultiplier;
    public bool doubleJump;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private bool isGrounded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        jumpAction = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        // Flip sprite
        if (moveInput.x > 0)
        {
            // Face right
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveInput.x < 0)
        {
            // Face left (flip on X-axis)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        // Jump
        if (jumpAction.triggered && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false; // prevent double jump until we land again
        }
    }

    void FixedUpdate()
    {
        bool isSprinting = sprintAction.ReadValue<float>() > 0;
        float currentSpeed = isSprinting ? speed * sprintMultiplier : speed;

        rb.linearVelocity = new Vector2(moveInput.x * currentSpeed, rb.linearVelocity.y);
    }

    // Detect ground using collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGround(collision))
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (IsGround(collision))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (IsGround(collision))
        {
            isGrounded = false;
        }
    }

    // Helper: Is this object the ground?
    bool IsGround(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ground")) return false;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // We're touching a surface from the top *and* falling
            if (doubleJump)
            {
                if (contact.normal.y > 0.5f)
                {
                    return true;
                }
            }
            else
            {
                if (contact.normal.y > 0.5f && rb.linearVelocity.y <= 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
