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

    public void SetUpdateLevel(int level, float exp)
    {
        selectedPlayer.level = level;
        selectedPlayer.exp = exp;
    }

    public void SetPlayerStatus(float def, float endurance,float damage, float damageBoom)
    {
        selectedPlayer.def = def;
        selectedPlayer.endurance = endurance;
        selectedPlayer.damage = damage;
        selectedPlayer.damageBoom = damageBoom;
    }

    public PlayerStatus GetSelectedPlayer()
    {
        return selectedPlayer;
    }
}
