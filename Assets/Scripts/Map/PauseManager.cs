using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;

    private bool isPaused = false;

    private void Start()
    {
        // Initialize button listeners
        if (resumeButton) resumeButton.onClick.AddListener(ResumeGame);
        if (settingsButton) settingsButton.onClick.AddListener(OpenSettings);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);

        // Ensure pause menu is hidden at start
        if (pauseMenuUI) pauseMenuUI.SetActive(false);
    }

    private void Update()
    {
        if (GameStateManager.Instance.IsGameOver() || GameStateManager.Instance.IsVictory())
            return;
        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject canva in canvases)
        {
            canva.SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        GameStateManager.Instance.SetState(GameState.Playing);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseUnPauseGame()
    {
        if (GameStateManager.Instance.IsGameOver() || GameStateManager.Instance.IsVictory())
            return;
        if (isPaused)
        {
            pauseMenuUI.SetActive(false);
            GameStateManager.Instance.SetState(GameState.Playing);
            Time.timeScale = 1f;
            isPaused = false;
        } else
        {
            GameObject[] canvases = GameObject.FindGameObjectsWithTag("Menu");
            foreach (GameObject canva in canvases)
            {
                canva.SetActive(false);
            }
            pauseMenuUI.SetActive(true);
            GameStateManager.Instance.SetState(GameState.Paused);
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    private void OpenSettings()
    {
        // TODO: Implement settings menu
        Debug.Log("Settings menu not implemented yet");
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 