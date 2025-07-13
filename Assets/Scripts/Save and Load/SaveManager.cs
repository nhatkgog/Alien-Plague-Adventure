using System.Collections;
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
        public static bool IsNewGame = false;

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
            StartCoroutine(DelayedLoad());
        }

        private IEnumerator DelayedLoad()
        {
            yield return new WaitForSeconds(0.1f); // Or yield return null for one frame
            saveManager = FindAllSaveManager();
            LoadGame();
        }
        public void NewGame()
        {
            gameData = new GameData();

        }
        public string GetLastSavedScene()
        {
            return gameData != null ? gameData.lastSceneName : null;
        }


        public void LoadGame()
        {
            gameData = fileDataHandler.Load();

            if (IsNewGame || gameData == null)
            {
                Debug.Log("New game started");
                NewGame();
            }
            else
            {
                // Load previously saved character
                if (!string.IsNullOrEmpty(gameData.selectedCharacterName))
                {
                    PlayerSelector.Instance.SetSelectedPlayerByName(gameData.selectedCharacterName);
                    Debug.Log("Loaded player from save: " + gameData.selectedCharacterName);
                }
                else
                {
                    Debug.LogWarning("No character found in save.");
                }
            }

            // Now load all component states
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
            gameData.lastSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            fileDataHandler.Save(gameData);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                SaveGame();
                Debug.Log("Game saved with 7");
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                LoadGame();
                Debug.Log("Game loaded with 8");
            }
        }

        private List<ISaveManager> FindAllSaveManager()
        {
            IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
            return new List<ISaveManager>(saveManagers);
        }
        public void RegisterSaveManager(ISaveManager manager)
        {
            if (!saveManager.Contains(manager))
                saveManager.Add(manager);
        }


    }
}
