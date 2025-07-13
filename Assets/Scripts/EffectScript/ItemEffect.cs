using UnityEngine;

namespace Assets.Scripts.Efect
{
    public class ItemEffect : ScriptableObject
    {
        public virtual void ExcecuteEffect(Transform _enemyPosition)
        {
            // Default implementation does nothing
            Debug.Log("Executing default item effect.");
        }
    }
}
