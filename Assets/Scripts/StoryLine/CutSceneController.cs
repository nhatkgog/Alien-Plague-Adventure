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
        canvasScenes[currentIndex].SetActive(false);
        currentIndex++;
        if (currentIndex < canvasScenes.Length) 
        {
            ShowScene(currentIndex);
        }
        else
        {
            Debug.Log("Cutscene complete!");
            SceneManager.LoadScene("GameLobby");
        }
    }

    void ShowScene(int index)
    {
        canvasScenes[index].SetActive(true);
    }
    
}
