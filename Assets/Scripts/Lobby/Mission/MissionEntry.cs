using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static MissionListHorizontalUI;

public class MissionEntry : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockIconSprite;
    [SerializeField] private FadeController fadeController;
    [SerializeField] private GameObject missionTab; 

    private string sceneToLoad;
    private MissionStatus status;

    public void Setup(Sprite image, string title, string description, string sceneName, MissionStatus status)
    {
        this.image.sprite = image;
        this.titleText.text = title;
        this.descriptionText.text = description;
        this.sceneToLoad = sceneName;
        this.status = status;

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();

            if (status == MissionStatus.Locked)
            {
                btn.interactable = false;

                CanvasGroup canvasGroup = missionTab.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0.5f;      
                    canvasGroup.interactable = false;
                    canvasGroup.blocksRaycasts = true;
                }

                if (lockImage != null)
                {
                    lockImage.sprite = lockIconSprite;
                    lockImage.gameObject.SetActive(true);
                }
                else
                {
                    if (lockImage != null)
                        lockImage.gameObject.SetActive(false);
                }
            }
            else
            {
                btn.interactable = true;

                CanvasGroup canvasGroup = missionTab.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 1f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;
                }

                if (lockImage != null)
                    lockImage.gameObject.SetActive(false);

                btn.onClick.AddListener(OnClick);
            }
        }
    }


    private void OnClick()
    {
        Debug.Log("Mission clicked → loading scene...");

        if (fadeController != null)
            fadeController.FadeToScene(sceneToLoad);

        if (GameStateManager.Instance != null)
            GameStateManager.Instance.SetState(GameState.Playing);
    }

    public string GetSceneName()
    {
        return sceneToLoad;
    }
}
