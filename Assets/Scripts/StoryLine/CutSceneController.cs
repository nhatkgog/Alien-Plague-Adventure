using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public static CutSceneController instance;
    private AudioSource audioSource;
    public GameObject[] canvasScenes;
    private int currentIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        ShowScene(currentIndex);
    }

    public void NextScene()
    {
        if (currentIndex + 1 >= canvasScenes.Length)
        {
            SceneManager.LoadScene("GameLobby");
            Debug.Log("Moving to Game Lobby....");
            return;
        }

        canvasScenes[currentIndex].SetActive(false);
        currentIndex++;
        ShowScene(currentIndex);
    }

    void ShowScene(int index)
    {
        canvasScenes[index].SetActive(true);
    }
    
}
