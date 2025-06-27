using UnityEngine;

public class Boom : MonoBehaviour
{
    private float explosionRadius;
    private float damage;
    private float knockbackForce;
    //public GameObject explosionEffect;
    public LayerMask damageLayer; // Layer chứa enemy

    private Animator animator;
    private bool hasExploded = false;

    private Rigidbody2D r;
    void Start()
    {

        Debug.Log("Boom created at " + transform.position);

        animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = PlayerSelector.Instance.GetSelectedPlayer().boomanimation;

        r = GetComponent<Rigidbody2D>();

        explosionRadius = InputSystemMovement.explosionRadius;
        damage = InputSystemMovement.damageBoom;
        knockbackForce = InputSystemMovement.knockbackForce;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & damageLayer) != 0 && !hasExploded)
        {
            if (r != null)
            {
                r.linearVelocity = Vector2.zero;
                r.isKinematic = true; 
            }
            Explode();
        }
    }


    void Explode()
    {
        if (hasExploded) return; // Tránh nổ nhiều lần
        hasExploded = true;

        // Gọi animation nổ
        if (animator != null)
        {
            animator.SetTrigger("Explosion");
            Debug.Log("co animation");
        }

        // Hiệu ứng nổ (tuỳ chọn)
        //if (explosionEffect != null)
        //{
        //    Instantiate(explosionEffect, transform.position, Quaternion.identity);
        //}

        // Xử lý sát thương + đẩy enemy
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Damage
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(Mathf.RoundToInt(damage));
                }

                // Knockback
                Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 forceDir = (hit.transform.position - transform.position).normalized;
                    rb.AddForce(forceDir * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        // Hủy bom sau 0.5–1s để animation nổ được hiển thị
        Destroy(gameObject, 0.5f);
    }

}
