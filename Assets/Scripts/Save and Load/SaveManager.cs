using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Save_and_Load
{
    public class SaveManager : MonoBehaviour
    {
        private GameData gameData;
        public static SaveManager instance;
        private List<ISaveManager> saveManager;
        private FileDataHandler fileDataHandler;
        [SerializeField] private string fileName;
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            else
            {
                instance = this;

            }
        }
        private void Start()
        {
            fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
            saveManager = FindAllSaveManager();
            LoadGame();

        }
        public void NewGame()
        {
            gameData = new GameData();

        }
        public void LoadGame()
        {
            gameData = fileDataHandler.Load();
            if (this.gameData == null)
            {
                Debug.Log("No game data found, starting a new game.");
                NewGame();
            }
            foreach (ISaveManager saveManager in this.saveManager)
            {
                saveManager.LoadData(gameData);
            }

        }
        public void SaveGame()
        {
            foreach (ISaveManager saveManager in this.saveManager)
            {
                saveManager.SaveData(gameData);
            }
            fileDataHandler.Save(gameData);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SaveGame();
                Debug.Log("Game saved with F5");
            }

            if (Input.GetKeyDown(KeyCode.F9))
            {
                LoadGame();
                Debug.Log("Game loaded with F9");
            }
        }

        private List<ISaveManager> FindAllSaveManager()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }
    }
}
