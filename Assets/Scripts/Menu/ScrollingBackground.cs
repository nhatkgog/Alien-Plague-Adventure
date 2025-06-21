using UnityEngine;
using UnityEngine.UI;

public class ScrollingAndZoomingBackground : MonoBehaviour
{
    public RawImage image;
    public Vector2 scrollSpeed = new Vector2(0.05f, 0f); 
    public float zoomSpeed = 1f; 
    public float zoomAmount = 0.05f; 

    private Vector2 originalSize;
    private float timer = 0f;

    void Start()
    {
        if (image != null)
            originalSize = image.uvRect.size;
    }

    void Update()
    {
        if (image != null)
        {

            image.uvRect = new Rect(image.uvRect.position + scrollSpeed * Time.deltaTime, image.uvRect.size);

            timer += Time.deltaTime * zoomSpeed;
            float zoomOffset = Mathf.Sin(timer) * zoomAmount;

            Vector2 newSize = originalSize + new Vector2(zoomOffset, zoomOffset);
            image.uvRect = new Rect(image.uvRect.position, newSize);
        }
    }
}
