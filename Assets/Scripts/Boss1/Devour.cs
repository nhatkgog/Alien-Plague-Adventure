using UnityEngine;

public class Devour : MonoBehaviour
{
    [SerializeField] GameObject hitPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<InputSystemMovement>();
            if (player != null)
            {
                float playerHp = player.currentHealth; // phải là instance, không static
                player.PlayerHurt(playerHp);
                Debug.Log("Fenrir Devour hit the player!");
            }

        }
        if (hitPrefab != null)
        {
            GameObject hit = Instantiate(hitPrefab, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
        }
    }
}
