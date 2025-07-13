using UnityEngine;

namespace Assets.Scripts.Efect
{
    [CreateAssetMenu(fileName = "Exp effect", menuName = "Data/Item effect/ Exp effect")]
    public class ExpEffect : ItemEffect
    {
        [SerializeField, Range(0f, 1f)] private float expPercentage;

        public override void ExcecuteEffect(Transform _target)
        {
            InputSystemMovement player = _target.GetComponent<InputSystemMovement>();
            if (player != null)
            {
                float currentExp = player.GetCurrentExp();
                float expToAdd = player.GetExpToLevelUp() * expPercentage;

                player.AddExp(expToAdd);
                Debug.Log($" Gained {expToAdd} EXP (now: {currentExp + expToAdd})");
            }
        }
    }
}
