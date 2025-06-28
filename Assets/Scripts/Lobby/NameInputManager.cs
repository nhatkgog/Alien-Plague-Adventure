using System.Collections;
using TMPro;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    [SerializeField] private GameObject enterNameCanvas;
    [SerializeField] private GameObject lobbyCanvas;

    [SerializeField] private TMP_InputField nameInputField; 
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_Text playerName;
    private Coroutine warningCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        warningText.text = "";
        nameInputField.Select();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            OnClickOK();
        }
    }

    public void OnClickOK()
    {
        SubmitName();
    }

    void SubmitName()
    {
        string yourName = nameInputField.text.Trim();
        warningText.text = "";

        if (!string.IsNullOrEmpty(yourName) && yourName != "Player")
        {
            playerName.text = yourName;
            PlayerPrefs.SetString("PlayerName", yourName);
            PlayerPrefs.Save();

            enterNameCanvas.SetActive(false);
            lobbyCanvas.SetActive(true);
        }
        else
        {
            if (warningCoroutine != null)
            {
                StopCoroutine(warningCoroutine);
            }

            warningCoroutine = StartCoroutine(ShowWarningTemporarily("Please enter a valid name!"));
        }
    }


    IEnumerator ShowWarningTemporarily(string message)
    {
        warningText.text = message;
        yield return new WaitForSeconds(10);
        warningText.text = "";
    }
}
