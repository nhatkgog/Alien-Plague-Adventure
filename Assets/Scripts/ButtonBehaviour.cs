using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour, IPointerEnterHandler
{
    public AudioClip hoverClip;
    public AudioClip clickSound;
    private Button button;

    void Awake()
    {
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
            SFXManager.Instance.PlayOneShot(hoverClip);
        }
    }

    private void PlayClickSound()
    {
        if (clickSound != null)
        {
            SFXManager.Instance.PlayOneShot(clickSound);
        }
    }
}
