using UnityEngine;

namespace Assets.Scripts.Save_and_Load
{
    public class CoinSaveManager : MonoBehaviour, ISaveManager
    {
        private void Awake()
        {
            if (SaveManager.instance != null)
            {
                SaveManager.instance.RegisterSaveManager(this);
            }
        }

        public void SaveData(GameData data)
        {
            data.missionCoinAmount = Coin.GetMissionTotal();
            Debug.Log("[CoinSaveManager] Saved: " + data.missionCoinAmount);
        }

        public void LoadData(GameData data)
        {
            Coin.missionCoinAmount = data.missionCoinAmount;
            Debug.Log("[CoinSaveManager] Loaded: " + Coin.missionCoinAmount);
        }
    }
}
