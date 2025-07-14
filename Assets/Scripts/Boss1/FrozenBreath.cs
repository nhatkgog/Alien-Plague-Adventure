using UnityEngine;

public class FrozenBreath : MonoBehaviour
{
    [Header("Thông số kỹ năng")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 15f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private float slowAmount = 0.5f;    // 50% tốc độ
    [SerializeField] private float slowDuration = 2f;    // thời gian bị làm chậm

    private float duration;
    private Vector3 direction;

    public void Initialize(float lifeTime)
    {
        duration = lifeTime;
        direction = transform.right.normalized;
        Destroy(gameObject, duration);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & hitLayers) != 0)
        {
            if (other.CompareTag("Player"))
            {
                var player = other.GetComponent<InputSystemMovement>();
                if (player != null)
                {
                    player.PlayerHurt(damage * Boss1.damageBonus);
                }
            }
            Freezable freeze = other.GetComponent<Freezable>();
            if (freeze != null)
            {
                freeze.ApplySlow(slowAmount, slowDuration);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + direction * 2f);
    }
}
