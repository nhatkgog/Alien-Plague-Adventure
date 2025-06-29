namespace Assets.Scripts.Save_and_Load
{
    public interface ISaveManager
    {
        void LoadData(GameData _data);
        void SaveData(GameData _data);
    }
}
