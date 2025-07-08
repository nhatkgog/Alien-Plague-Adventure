using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusic : MonoBehaviour
{
    public static MainMenuMusic Instance;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.loop = true;
            audioSource.Play();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Scence1" || scene.name ==  "Scene2")
        {
            audioSource.Stop(); 
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
