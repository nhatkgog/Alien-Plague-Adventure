using UnityEngine;

[CreateAssetMenu]
public class PlayerStatus : ScriptableObject
{
    public string characterName;
    public Sprite characterIcon;
    
    public float moveSpeed;
    public float maxHealth;
    public float def;
    public float endurance;

    public int level;
    public float exp;
    public int statPoints;

    public float money;
    //bullet
    public float damage;
    public int maxBulletQuantity;
    public float shootDelay;
    //boom
    public float damageBoom;
    public float explosionRadius;
    public int maxBoomQuatity;
    public float knockbackForce;
    public RuntimeAnimatorController boomanimation;

    public RuntimeAnimatorController animatorController;

    public GameObject bulletPrefab;
}
