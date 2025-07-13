using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    [SerializeField] GameObject hitPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<InputSystemMovement>();
            if (player != null)
            {
                player.PlayerHurt(damage);
            }

        }
        if (hitPrefab != null)
        {
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
    }
}
