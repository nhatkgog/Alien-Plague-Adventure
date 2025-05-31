using UnityEngine;

[CreateAssetMenu]
public class PlayerDatabase : ScriptableObject
{
    public PlayerStatus[] players;
    public int PlayerCount
    {
        get
        {
            return players.Length;
        }
    }

    public PlayerStatus GetPlayer(int index)
    {
        return players[index];
    }

}
