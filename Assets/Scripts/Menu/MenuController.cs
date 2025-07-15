using Assets.Scripts.Save_and_Load;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuCanvas;
    [SerializeField] public GameObject loadGameCanvas;
    [SerializeField] public GameObject settingsCanvas;
    [SerializeField] private FadeController fadeController;

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickNewGame()
    {
        SaveManager.IsNewGame = true;
        fadeController.FadeToScene("StoryLine");
    }

    public void OnClickSettings()
    {
        mainMenuCanvas.SetActive(false);
        loadGameCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void OnClickBack()
    {
        mainMenuCanvas.SetActive(true);
        loadGameCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void OnClickLoadGame()
    {
        mainMenuCanvas.SetActive(false);
        loadGameCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
        SaveManager.IsNewGame = false; // mark as continuing
        string savedScene = SaveManager.instance.GetLastSavedScene(); // custom method you'll add next

        if (!string.IsNullOrEmpty(savedScene))
        {
            fadeController.FadeToScene(savedScene); // fade to the scene saved in GameData
        }
        else
        {
            Debug.LogWarning("No saved scene found. Loading default game scene.");
            fadeController.FadeToScene("GameLobby"); // fallback scene
        }
    }

    public void OnClickExit()
    {
        Debug.Log("Game is exiting...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }

}
