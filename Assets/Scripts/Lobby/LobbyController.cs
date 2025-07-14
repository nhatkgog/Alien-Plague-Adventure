using TMPro;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    [SerializeField] public GameObject lobbyCanvas;
    [SerializeField] public GameObject armorCanvas;
    [SerializeField] public GameObject weaponCanvas;
    [SerializeField] public GameObject missionCanvas;
    [SerializeField] public GameObject enterNameCanvas;
    [SerializeField] public GameObject topBarCanvas;
    [SerializeField] public GameObject textBubbleCanvas;
    [SerializeField] public GameObject settingPopupCanvas;

    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text playerMoney;
    [SerializeField] private TMP_Text playerChoose;

    void Awake()
    {
        topBarCanvas.SetActive(true);
        textBubbleCanvas.SetActive(true);
        playerChoose.text = "Hello";
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

    }


    void Update()
    {
        UpdateTextBubbleVisibility();
    }

    public void OnClickBack()
    {
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(true);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
        settingPopupCanvas.SetActive(false);
        UpdateTextBubbleVisibility();
    }

    public void OnClickSettingsPopUp()
    {
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(true);
        settingPopupCanvas.SetActive(true);
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
        settingPopupCanvas.SetActive(false);
    }

    public void OnClickWeapon()
    {
        textBubbleCanvas.SetActive(false);
        topBarCanvas.SetActive(true);
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(true);
        missionCanvas.SetActive(false);
        settingPopupCanvas.SetActive(false);
    }

    public void OnClickMission()
    {
        if (PlayerSelector.Instance == null || PlayerSelector.Instance.selectedPlayerOriginal == null)
        {
            UpdateTextBubbleVisibility();
            Debug.Log("You must select a character before entering missions.");
            return;
        }

        textBubbleCanvas.SetActive(true);
        topBarCanvas.SetActive(false);
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(true);
        settingPopupCanvas.SetActive(false);
    }



    private void UpdateTextBubbleVisibility()
    {
        textBubbleCanvas.SetActive(true); 

        if (PlayerSelector.Instance != null && PlayerSelector.Instance.selectedPlayerOriginal != null)
        {
            playerChoose.text = "Hello";
        }
        else
        {
            playerChoose.text = "Select first!";
        }
    }


}
