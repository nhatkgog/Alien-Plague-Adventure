using UnityEngine;

public class IceBullet : MonoBehaviour
{
    private Vector3 movementDirection;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] GameObject hitPrefab;

    void Start()
    {
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * speed * Time.deltaTime;
    }

    private float speed;

    public void SetMovementDirection(Vector3 direction, float bulletSpeed)
    {
        movementDirection = direction.normalized;
        speed = bulletSpeed;

        float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
                    player.PlayerHurt(damage*Boss1.damageBonus);
                }
            }
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
            // Nếu không cần xuyên qua thì destroy
            Destroy(gameObject);
        }
    }
}
