using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MissionEntry : MonoBehaviour
{
    public Image image;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    private string sceneToLoad;
    [SerializeField] private FadeController fadeController;
    public void Setup(Sprite image, string title, string description, string sceneName)
    {
        this.image.sprite = image;
        this.titleText.text = title;
        this.descriptionText.text = description;
        this.sceneToLoad = sceneName;

        Button btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners(); 
            btn.onClick.AddListener(OnClick);
        }
    }

    private void OnClick()
    {
        Debug.Log("Moving to game scene.....");
        fadeController.FadeToScene(sceneToLoad);
    }
}
