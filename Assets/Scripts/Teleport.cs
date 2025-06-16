using UnityEngine;
using Unity.Cinemachine;

public class Teleport : MonoBehaviour
{
    public Transform destination;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Transform playerTransform = other.transform;
            Vector3 offset = destination.position - playerTransform.position;

            // Move player
            playerTransform.position = destination.position;

            // Get the CinemachineBrain (usually on the main camera)
            CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
            if (brain != null)
            {
                // Get the currently active virtual camera as CinemachineCamera (the base class)
                CinemachineCamera vcam = brain.ActiveVirtualCamera as CinemachineCamera;
                if (vcam != null)
                {
                    vcam.OnTargetObjectWarped(playerTransform, offset);
                }
            }
        }
    }
}
