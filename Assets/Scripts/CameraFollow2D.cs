using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;
    public SpriteRenderer background;

    private Vector2 minCameraPos;
    private Vector2 maxCameraPos;

    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private Camera cam;
    private float camHeight;
    private float camWidth;

    void Start()
    {
        cam = Camera.main;
        camHeight = cam.orthographicSize;
        camWidth = camHeight * cam.aspect;

        // Get background bounds
        Bounds bgBounds = background.bounds;
        minCameraPos = new Vector2(bgBounds.min.x, bgBounds.min.y);
        maxCameraPos = new Vector2(bgBounds.max.x, bgBounds.max.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);

            // Smoothly move the camera
            Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            // Clamp the camera position to the background edges
            float clampedX = Mathf.Clamp(smoothPos.x, minCameraPos.x + camWidth, maxCameraPos.x - camWidth);
            float clampedY = Mathf.Clamp(smoothPos.y, minCameraPos.y + camHeight, maxCameraPos.y - camHeight);

            transform.position = new Vector3(clampedX, clampedY, smoothPos.z);
        }
    }
}
