using UnityEngine;

public class Enemy1AnimationTrigger : MonoBehaviour
{
    private Enemy1 enemy => GetComponentInParent<Enemy1>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }
}
