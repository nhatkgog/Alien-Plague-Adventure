using UnityEngine;

public class Enemy1AnimationTrigger : MonoBehaviour
{
    private Enemy1 enemy => GetComponentInParent<Enemy1>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
    private void AttackTrigger()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attckCheckRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                hitCollider.GetComponent<InputSystemMovement>().PlayerHurt(enemy.attackDamage);
            }
        }
    }
}
