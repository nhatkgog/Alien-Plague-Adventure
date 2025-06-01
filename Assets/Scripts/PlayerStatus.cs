using UnityEngine;

[CreateAssetMenu]
public class PlayerStatus : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;
    public float moveSpeed;
    public float maxHealth;
    public float damage;
    public int level;
    public float exp;
    public int maxBulletQuantity;
    public float shootDelay;
    public RuntimeAnimatorController animatorController;
}
