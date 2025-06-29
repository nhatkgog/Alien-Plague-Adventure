using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    public GameObject[] canvasScenes;
    private int currentIndex = 0;

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
