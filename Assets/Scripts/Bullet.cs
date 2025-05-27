using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime); 
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision (e.g., damage enemies)
        Destroy(gameObject);
    }
}
