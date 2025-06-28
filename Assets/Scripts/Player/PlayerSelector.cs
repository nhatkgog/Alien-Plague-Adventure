using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector Instance;

    public PlayerStatus selectedPlayer;

    [Header("Gán ScriptableObject gốc ở đây")]
    public PlayerStatus selectedPlayerOriginal;

    private PlayerStatus runtimePlayer;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
            runtimePlayer = Instantiate(selectedPlayerOriginal);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public PlayerStatus GetSelectedPlayer()
    {
        return runtimePlayer;
    }

    public void ResetPlayerToOriginal()
    {
        runtimePlayer = Instantiate(selectedPlayerOriginal);
    }

    public void PartialResetPlayer()
    {
        int savedLevel = runtimePlayer.level;
        float savedExp = runtimePlayer.exp;

        runtimePlayer = Instantiate(selectedPlayerOriginal);
        runtimePlayer.level = savedLevel;
        runtimePlayer.exp = savedExp;
        runtimePlayer.statPoints = (savedLevel - 1) * 10;
    }

    public void SetPlayerStatus(float def, float endurance, float damage, float damageBoom)
    {
        runtimePlayer.def = def;
        runtimePlayer.endurance = endurance;
        runtimePlayer.damage = damage;
        runtimePlayer.damageBoom = damageBoom;
    }

    public void SetUpdateLevel(int level, float exp)
    {
        runtimePlayer.level = level;
        runtimePlayer.exp = exp;
    }

    public void SetStatPoint(int statpoint)
    {
        runtimePlayer.statPoints = statpoint;
    }

    public void SetSelectedPlayer(PlayerStatus player)
    {
        selectedPlayerOriginal = player;
        runtimePlayer = Instantiate(selectedPlayerOriginal);
    }
}
