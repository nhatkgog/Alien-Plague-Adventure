using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("isShooting");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Ignore collision between player and bullet
        Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
        Collider2D playerCollider = GetComponent<Collider2D>();

        if (bulletCollider != null && playerCollider != null)
        {
            Physics2D.IgnoreCollision(bulletCollider, playerCollider);
        }
    }

    void Attack()
    {

    }
}
