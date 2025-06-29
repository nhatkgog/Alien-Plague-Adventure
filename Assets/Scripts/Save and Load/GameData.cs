namespace Assets.Scripts.Save_and_Load
{
    [System.Serializable]
    public class GameData
    {
        public float currency;
        public float health;
        public int bulletCount;
        public int boomCount;


        public GameData()
        {
            this.currency = 0;
        }

    }
}
