using Assets.Scripts.Save_and_Load;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public PlayerDatabase PlayerDB;

    public TMP_Text namePlayer;
    public SpriteRenderer artworkSprite;

    private int selectOption = 0;

    private GameData gameData;
    private FileDataHandler fileDataHandler;
    [SerializeField] private string fileName = "saving.json";

    void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        gameData = fileDataHandler.Load();

        if (!SaveManager.IsNewGame &&
            PlayerSelector.Instance != null &&
            gameData != null &&
            !string.IsNullOrEmpty(gameData.selectedCharacterName))
        {
            string loadedName = gameData.selectedCharacterName;
            PlayerSelector.Instance.SetSelectedPlayerByName(loadedName);
            selectOption = PlayerDB.GetIndexByName(loadedName);
            updatePlayer(selectOption);
            Save(); // Save PlayerPrefs
        }
        else if (PlayerPrefs.HasKey("selectOption"))
        {
            Load();
            updatePlayer(selectOption);
        }
        else
        {
            selectOption = 0;
            updatePlayer(selectOption);
        }
    }

    // Update is called once per frame
    void Update()
    {
        updatePlayer(selectOption);
    }

    public void nextOption()
    {
        selectOption++;

        if (selectOption >= PlayerDB.PlayerCount)
        {
            selectOption = 0;
        }

        updatePlayer(selectOption);
        Save();
    }

    public void backOption()
    {
        selectOption--;

        if (selectOption < 0)
        {
            selectOption = PlayerDB.PlayerCount - 1;
        }

        updatePlayer(selectOption);
        Save();
    }

    private void updatePlayer(int selectOption)
    {
        PlayerStatus status = PlayerDB.GetPlayer(selectOption);
        artworkSprite.sprite = status.characterIcon;
        namePlayer.text = status.name;
    }

    private void Load()
    {
        selectOption = PlayerPrefs.GetInt("selectOption");
    }

    private void Save()
    {
        PlayerPrefs.SetInt("selectOption", selectOption);
    }

    public void ConfirmSelection()
    {
        PlayerStatus selected = PlayerDB.GetPlayer(selectOption);
        PlayerSelector.Instance.SetSelectedPlayer(selected);
    }

    public void changeScene()
    {
        PlayerStatus selected = PlayerDB.GetPlayer(selectOption);
        PlayerSelector.Instance.SetSelectedPlayer(selected);
        SceneManager.LoadScene("Scence1");
    }

    public void SaveCompletedMission(int missionIndex)
    {
        gameData = fileDataHandler.Load();
        if (gameData == null)
        {
            gameData = new GameData();
        }

        if (missionIndex > gameData.completedMissionIndex)
        {
            gameData.completedMissionIndex = missionIndex;
            fileDataHandler.Save(gameData);
            Debug.Log($"Saved completed mission index: {missionIndex}");
        }
    }
}
