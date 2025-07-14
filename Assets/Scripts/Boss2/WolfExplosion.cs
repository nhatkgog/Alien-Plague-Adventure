using Unity.VisualScripting;
using UnityEngine;

public class WolfExplosion : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage = 40f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Transform player;
    private bool isExploding = false;

    public Transform groundCheck;
    public float groundCheckDistance;
    public Transform wallCheck;
    public float wallCheckDistance;
    public LayerMask whatIsGround;
    public int facingDir { get; private set; } = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Start()
    {
        Invoke(nameof(Explode), 3f); // Auto explode after delay

        Collider2D myCol = GetComponent<Collider2D>();
        var others = GameObject.FindGameObjectsWithTag("WolfExplosion"); // Gắn tag này vào prefab

        foreach (var other in others)
        {
            if (other == gameObject) continue;
            Collider2D col = other.GetComponent<Collider2D>();
            if (col != null)
                Physics2D.IgnoreCollision(myCol, col);
        }
    }
    private void Update()
    {
        IsGroundDetected();
        IsWallDetected();
    }

    private void FixedUpdate()
    {
        if (isExploding || player == null) return;

        // Di chuyển về phía player theo trục X
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Lật hướng sprite
        if (direction.x < 0)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        else
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isExploding) return;

        if (other.CompareTag("Player"))
        {
            other.GetComponent<InputSystemMovement>()?.PlayerHurt(damage);
            Debug.Log("No");
            Explode();
        }
    }

    private void Explode()
    {
        if (isExploding) return;
        isExploding = true;

        rb.linearVelocity = Vector2.zero;
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);

        Destroy(gameObject); // Remove the wolf
    }

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
}
