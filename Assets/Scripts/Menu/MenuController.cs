using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] public GameObject mainMenuCanvas;
    [SerializeField] public GameObject loadGameCanvas;
    [SerializeField] public GameObject settingsCanvas;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnClickNewGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    public void OnClickSettings()
    {
        mainMenuCanvas.SetActive(false);
        loadGameCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
    }

    public void OnClickBack()
    {
        mainMenuCanvas.SetActive(true);
        loadGameCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void OnClickLoadGame()
    {
        mainMenuCanvas.SetActive(false);
        loadGameCanvas.SetActive(true);
        settingsCanvas.SetActive(false);
    }

    public void OnClickExit()
    {
        Debug.Log("Game is exiting...");
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
    }
}
