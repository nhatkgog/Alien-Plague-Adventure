using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector Instance;

    //public PlayerStatus selectedPlayer;

    [Header("Gán ScriptableObject gốc ở đây")]
    public PlayerStatus selectedPlayerOriginal;

    public PlayerStatus runtimePlayer;

    [Header("List of all characters")]
    public List<PlayerStatus> allCharacters;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        allCharacters = new List<PlayerStatus>(Resources.LoadAll<PlayerStatus>("DataPlayer"));

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

    public void SetMoney(float money)
    {
        runtimePlayer.money = money;
    }

    public void SetBoomCount(int boom)
    {
        runtimePlayer.maxBoomQuatity = boom;
    }

    public void SetSelectedPlayer(PlayerStatus player)
    {
        selectedPlayerOriginal = player;
        runtimePlayer = Instantiate(selectedPlayerOriginal);
    }
    public void SetLevelUp(float gainedExp)
    {
        runtimePlayer.exp += gainedExp;

        while (true)
        {
            float expNeeded = Mathf.Pow(10, runtimePlayer.level);

            if (runtimePlayer.exp >= expNeeded)
            {
                runtimePlayer.exp -= expNeeded;
                runtimePlayer.level++;

                runtimePlayer.statPoints += 10;

                var playerScript = GameObject.FindWithTag("Player")?.GetComponent<InputSystemMovement>();
                if (playerScript != null)
                {
                    playerScript.HealToFull();
                }

                Debug.Log($"Level Up! New level: {runtimePlayer.level}");
            }
            else
            {
                break;
            }
        }
    }


    public void SetSelectedPlayerByName(string name)
    {
        Debug.Log("Looking for character: " + name);
        Debug.Log("Character list count: " + allCharacters.Count);
        foreach (var character in allCharacters)
        {
            Debug.Log("Checking character: " + character.characterName);
            if (character.characterName.Trim().Equals(name.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                SetSelectedPlayer(character);
                return;
            }
        }
        Debug.LogWarning("Selected character not found: " + name);
    }


}
