using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector Instance;

    public PlayerStatus selectedPlayer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // giữ lại qua các scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSelectedPlayer(PlayerStatus player)
    {
        selectedPlayer = player;
    }

    public PlayerStatus GetSelectedPlayer()
    {
        return selectedPlayer;
    }
}
