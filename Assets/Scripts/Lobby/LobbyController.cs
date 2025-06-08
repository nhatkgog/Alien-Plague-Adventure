using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyController : MonoBehaviour
{
    [SerializeField] public GameObject lobbyCanvas;
    [SerializeField] public GameObject armorCanvas;
    [SerializeField] public GameObject weaponCanvas;
    [SerializeField] public GameObject missionCanvas;

    void Start()
    {
        
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
        armorCanvas.SetActive(true);
        weaponCanvas.SetActive(false);
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
