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

    [SerializeField] private TMP_Text playerName;

    void Start()
    {
        string name = PlayerPrefs.GetString("PlayerName", "Player");
        if (PlayerPrefs.GetString("PlayerName") == null)
        {
            enterNameCanvas.SetActive(true);
        }
        else 
        {
            playerName.text = name; 
        }

    }

    void Update()
    {

    }

    public void OnClickBack()
    {
        lobbyCanvas.SetActive(true);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
    }

    public void OnClickArmor()
    {
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(true);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(false);
    }

    public void OnClickWeapon()
    {
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(true);
        missionCanvas.SetActive(false);
    }

    public void OnClickMission()
    {
        lobbyCanvas.SetActive(false);
        armorCanvas.SetActive(false);
        weaponCanvas.SetActive(false);
        missionCanvas.SetActive(true);
    }
}
