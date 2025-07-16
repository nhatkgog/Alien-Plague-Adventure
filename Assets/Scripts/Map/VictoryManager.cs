using Assets.Scripts.Save_and_Load;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryManager : MonoBehaviour
{
    [Header("Victory Menu")]
    [SerializeField] private GameObject victoryUI;

    [SerializeField] private Button continueButton;
    [SerializeField] private Button lobbyButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private TMP_Text endCoinText;

    public static float MissionCoinReward = 0f;
    public static bool OpenMissionTab = false;

    [Header("SFX")]
    [SerializeField] private AudioClip victoryClip;


    private void Start()
    {
        victoryUI.SetActive(false);

        continueButton.onClick.AddListener(OnContinue);
        lobbyButton.onClick.AddListener(OnReturnToLobby);
        quitButton.onClick.AddListener(OnQuit);
    }

    public void ShowVictory()
    {
        if (victoryClip != null)
            SFXManager.Instance.PlayOneShot(victoryClip);
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("Menu");
        foreach (GameObject canva in canvases)
        {
            canva.SetActive(false);
        }
        float missionEarned = Coin.GetMissionTotal();
        Debug.Log($"Victory! Total mission coin earned: {missionEarned}");

        MissionCoinReward = missionEarned;
        endCoinText.text = $"You earned: {missionEarned}";
        victoryUI.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Victory);
        Time.timeScale = 0f;
    }

    private int SceneToMissionIndex(string sceneName)
    {
        for (int i = 0; i < MissionListHorizontalUI.Instance.missions.Count; i++)
        {
            if (MissionListHorizontalUI.Instance.missions[i].sceneName == sceneName)
                return i;
        }
        return -1;
    }

    private void OnContinue()
    {
        Time.timeScale = 1f;
        OpenMissionTab = true;

        float savedCoins = PlayerPrefs.GetFloat("TotalCoins", 0f);
        savedCoins += MissionCoinReward;
        PlayerPrefs.SetFloat("TotalCoins", savedCoins);
        PlayerPrefs.Save();

        // Save mission progress to JSON
        var fileHandler = new FileDataHandler(Application.persistentDataPath, "saving.json");
        GameData gameData = fileHandler.Load();

        if (gameData == null)
        {
            gameData = new GameData();
        }

        // 🚀 Update mission index only if it's progressing forward
        int currentMissionIndex = SceneToMissionIndex(SceneManager.GetActiveScene().name);
        if (currentMissionIndex > gameData.completedMissionIndex)
        {
            gameData.completedMissionIndex = currentMissionIndex;
            fileHandler.Save(gameData);
        }

        // Reset for next mission
        Coin.ResetMissionTotal();
        MissionCoinReward = 0f;

        SceneManager.LoadScene("GameLobby");
    }


    private void OnReturnToLobby()
    {
        Time.timeScale = 1f;
        Coin.ResetMissionTotal();
        SceneManager.LoadScene("GameLobby");
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.SetState(GameState.Playing);
        }
    }

    private void OnQuit()
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
