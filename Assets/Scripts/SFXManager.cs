using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    private AudioSource sfxSource; // One-shot and looping
    private AudioSource loopSource; // Only for loops walk/run

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true;
    }

    public void PlayOneShot(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayLoop(AudioClip clip)
    {
        if (clip == null) return;

        if (loopSource.clip != clip || !loopSource.isPlaying)
        {
            loopSource.clip = clip;
            loopSource.Play();
        }
    }

    public void StopLoop()
    {
        if (loopSource.isPlaying)
        {
            loopSource.Stop();
            loopSource.clip = null;
        }
    }
}
