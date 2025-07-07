using TMPro;
using UnityEngine;

public class CoinDisplayUI : MonoBehaviour
{
    public TMP_Text missionCoinText;

    void Start()
    {
        if (missionCoinText != null)
        {
            missionCoinText.text = Coin.GetMissionTotal().ToString();
            Debug.Log("Loaded coin UI: " + Coin.GetMissionTotal());
        }
        else
        {
            Debug.LogWarning("CoinDisplayUI: missionCoinText not assigned.");
        }
    }
}
