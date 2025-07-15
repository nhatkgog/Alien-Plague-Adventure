using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    [SerializeField] private float damage = 15f;
    [SerializeField] GameObject hitPrefab;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("va cham");

        if (other.CompareTag("Player"))
        {
            Debug.Log("da co");

            var player = other.GetComponent<InputSystemMovement>();
            if (player != null)
            {
                player.PlayerHurt(damage);
            }
            Debug.Log("take dame");

        }
        if (hitPrefab != null)
        {
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
    }
}
