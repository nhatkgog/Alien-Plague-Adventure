using Unity.VisualScripting;
using UnityEngine;

public class DivineAscension : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float damage = 50f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 moveDirection;

    private void Start()
    {
        SetDirectionAndRotation();
    }
    private void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
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
                    player.PlayerHurt(damage);
                }

            }
            GameObject hit = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1f);
            Destroy(gameObject);

        }
    }
    private void SetDirectionAndRotation()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Tính vector hướng từ meteor tới player
        moveDirection = (player.transform.position - transform.position).normalized;

        // Tính góc xoay theo hướng bay
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}
