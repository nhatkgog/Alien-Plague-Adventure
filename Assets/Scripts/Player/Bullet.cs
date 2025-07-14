using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;
    public float damage;
    public LayerMask damageLayer;

    private Rigidbody2D rb;
    void Start()
    {
        damage = InputSystemMovement.damage;
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & damageLayer) != 0)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Mathf.RoundToInt(damage));
                }
                var boss1 = other.GetComponent<Boss1>();
                if (boss1 != null)
                {
                    boss1.TakeDamage(Mathf.RoundToInt(damage));
                }
                var boss2 = other.GetComponent<Boss2>();
                if (boss2 != null)
                {
                    boss2.TakeDamage(Mathf.RoundToInt(damage));
                }
            }

            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.isKinematic = true;
            }

            Destroy(gameObject);
        }
    }
}
