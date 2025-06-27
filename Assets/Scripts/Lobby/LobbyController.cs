using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [SerializeField] public GameObject lobbyCanvas;
    [SerializeField] public GameObject armorCanvas;
    [SerializeField] public GameObject weaponCanvas;
    [SerializeField] public GameObject missionCanvas;
    [SerializeField] public GameObject enterNameCanvas;
    [SerializeField] public GameObject topBarCanvas;
    [SerializeField] public GameObject textBubbleCanvas;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerMoney;

    void Awake()
    {
        topBarCanvas.SetActive(true);
    }
    void Start()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Player");
        if (string.IsNullOrWhiteSpace(name) || name == "Player")
        {
            enterNameCanvas.SetActive(true);
            lobbyCanvas.SetActive(false);
        }
        else
        {
            playerName.text = name;
            enterNameCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
        }

        float savedCoins = PlayerPrefs.GetFloat("TotalCoins", 0f);
        if (VictoryManager.MissionCoinReward > 0f)
        {
            savedCoins += VictoryManager.MissionCoinReward;
            PlayerPrefs.SetFloat("TotalCoins", savedCoins);
            PlayerPrefs.Save();
            VictoryManager.MissionCoinReward = 0f;
        }

        playerMoney.text = savedCoins.ToString();

        if (VictoryManager.OpenMissionTab)
        {
            VictoryManager.OpenMissionTab = false;
            OnClickMission();
        }
        else
        {
            OnClickBack();
        }

        UpdateTextBubbleVisibility();
    }


    void Update()
    {

    }

    public void OnClickBack()
    {
        UpdateTextBubbleVisibility();
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(true);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
    }

    public void OnClickArmor()
    {
        textBubbleCanvas.SetActive(false);
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(true);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
    }

    public void OnClickWeapon()
    {
        textBubbleCanvas.SetActive(false);
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(true);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
    }

    public void OnClickMission()
    {
        textBubbleCanvas.SetActive(false);
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(true);
    }

    private void UpdateTextBubbleVisibility()
    {
        if (PlayerSelector.Instance != null)
        {
            bool isSelected = PlayerSelector.Instance.selectedPlayer != null;
            textBubbleCanvas.SetActive(isSelected);
        }
        else
        {
            textBubbleCanvas.SetActive(false);
        }
    }

}
