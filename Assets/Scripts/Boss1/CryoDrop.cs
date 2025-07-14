using UnityEngine;

public class CryoDrop : MonoBehaviour
{
    [Header("Thuộc tính rơi")]
    [SerializeField] private float fallSpeed = 10f;

    [Header("Thuộc tính sát thương")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] GameObject hitPrefab;

    void Update()
    {
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);
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
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
            // Dù là chạm Player hay Ground đều bị hủy
            Destroy(gameObject);
        }
    }
}
