using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverClip;
    public AudioClip clickSound;
    public AudioSource audioSource;
    private Button button;

    void Awake()
    {
        audioSource.playOnAwake = false;

        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null)
        {
            audioSource.PlayOneShot(hoverClip);
        }
        
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
