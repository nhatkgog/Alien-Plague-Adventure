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

    [SerializeField] private TMP_Text missionCoinsText;

    public static float MissionCoinReward = 0f;
    public static bool OpenMissionTab = false;

    private void Start()
    {
        victoryUI.SetActive(false);

        continueButton.onClick.AddListener(OnContinue);
        lobbyButton.onClick.AddListener(OnReturnToLobby);
        quitButton.onClick.AddListener(OnQuit);
    }

    public void ShowVictory()
    {
        float missionEarned = Coin.GetMissionTotal();
        Debug.Log($"Victory! Total mission coin earned: {missionEarned}");

        MissionCoinReward = missionEarned;
        missionCoinsText.text = $"You earned: {missionEarned}";
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
    }


    private void OnContinue()
    {
        Time.timeScale = 1f;
        OpenMissionTab = true;

        // Save to PlayerPrefs
        float savedCoins = PlayerPrefs.GetFloat("TotalCoins", 0f);
        savedCoins += MissionCoinReward;
        PlayerPrefs.SetFloat("TotalCoins", savedCoins);
        PlayerPrefs.Save();

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
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
