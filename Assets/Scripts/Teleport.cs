using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform destination; // Assign your destination in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = destination.position;
        }
    }
}
