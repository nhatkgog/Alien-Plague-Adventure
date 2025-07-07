using UnityEngine;

namespace Assets.Scripts.Efect
{
    [CreateAssetMenu(fileName = "Heal effect", menuName = "Data/Item effect/ Heal effect")]
    public class HealEffect : ItemEffect
    {
        [Range(0f, 1f)]
        [SerializeField] private float healPercentage;

        public override void ExcecuteEffect(Transform _target)
        {
            InputSystemMovement player = _target.GetComponent<InputSystemMovement>();
            if (player != null)
            {
                int healAmount = Mathf.RoundToInt(player.GetMaxHealth() * healPercentage);
                player.Heal(healAmount);
            }
        }

    }
}
