using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over Menu")]
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button lobbyButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TextMeshProUGUI scoreText; // If you want to show final score

    [Header("SFX")]
    [SerializeField] private AudioClip gaveOverClip;

    private void Start()
    {
        // Initialize button listeners
        if (retryButton) retryButton.onClick.AddListener(RetryLevel);
        if (lobbyButton) lobbyButton.onClick.AddListener(ReturnToLobby);
        if (quitButton) quitButton.onClick.AddListener(QuitGame);

        // Ensure game over menu is hidden at start
        if (gameOverUI) gameOverUI.SetActive(false);
    }

    public void ShowGameOver(int finalScore = 0)
    {
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject canva in canvases)
        {
            canva.SetActive(false);
        }
        gameOverUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.GameOver);

        if (gaveOverClip != null)
            SFXManager.Instance.PlayOneShot(gaveOverClip);

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
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }

    private void ReturnToLobby()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameLobby");
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }

    private void QuitGame()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 