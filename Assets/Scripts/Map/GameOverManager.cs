using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over Menu")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI scoreText; // If you want to show final score

    private void Start()
    {
        // Initialize button listeners
        if (retryButton) retryButton.onClick.AddListener(RetryLevel);
        if (mainMenuButton) mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);

        // Ensure game over menu is hidden at start
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    public void ShowGameOver(int finalScore = 0)
    {
        gameOverUI.SetActive(true);
        if (scoreText != null)
        {
            scoreText.text = $"Final Score: {finalScore}";
        }
        Time.timeScale = 0f;
    }

    private void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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