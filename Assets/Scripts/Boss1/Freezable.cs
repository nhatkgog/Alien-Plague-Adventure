using UnityEngine;

public class Freezable : MonoBehaviour
{
    private float originalSpeed;
    private bool isFrozen = false;

    public void ApplySlow(float slowAmount, float duration)
    {
        if (isFrozen) return;

        var movement = GetComponent<InputSystemMovement>();
        if (movement != null)
        {
            originalSpeed = movement.speed;
            movement.speed *= slowAmount; // ví dụ slowAmount = 0.5f → giảm 50%
            isFrozen = true;
            Invoke(nameof(RemoveSlow), duration);
        }
    }

    private void RemoveSlow()
    {
        var movement = GetComponent<InputSystemMovement>();
        if (movement != null)
        {
            movement.speed = originalSpeed;
        }
        isFrozen = false;
    }
}

