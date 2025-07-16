using System.Collections.Generic;

namespace Assets.Scripts.Save_and_Load
{
    [System.Serializable]
    public class GameData
    {
        public float currency;
        public float health;
        public int bulletCount;
        public int boomCount;
        public int statPoint;

        public string lastSceneName = "GameLobby";

        public int level;
        public float exp;

        public string selectedCharacterName;
        public int completedMissionIndex;

        public float missionCoinAmount;

        public SerializableDictionary<string, int> inventory;
        public List<string> equipmentId;
        public GameData()
        {
            this.currency = 0;
            inventory = new SerializableDictionary<string, int>();
            completedMissionIndex = -1;
        }

    }

}
