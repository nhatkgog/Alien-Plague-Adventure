using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public static MainMenuMusic instance;
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            if (audioSource != null && !audioSource.isPlaying)
            {
                audioSource.loop = true;
                audioSource.playOnAwake = false;
                audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
