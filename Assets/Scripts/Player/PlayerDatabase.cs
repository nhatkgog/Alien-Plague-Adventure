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
    public int GetIndexByName(string name)
    {
        for (int i = 0; i < PlayerCount; i++)
        {
            if (GetPlayer(i).characterName == name)
                return i;
        }
        return 0; // fallback to default
    }

}
