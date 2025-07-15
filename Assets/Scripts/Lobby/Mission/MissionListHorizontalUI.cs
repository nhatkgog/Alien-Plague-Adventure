using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Save_and_Load;

public class MissionListHorizontalUI : MonoBehaviour
{
    public static MissionListHorizontalUI Instance;

    public GameObject missionEntryPrefab;
    public Transform content;

    [System.Serializable]
    public class MissionData
    {
        public Sprite image;
        public string title;
        public string description;
        public string sceneName;
        public MissionStatus status;
    }

    public List<MissionData> missions;

    [SerializeField] private string fileName = "saving.json";

    void Start()
    {
        if (missions.Count == 0) return;

        int completedIndex = -1;
        var fileHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        GameData gameData = fileHandler.Load();

        if (gameData != null)
        {
            completedIndex = gameData.completedMissionIndex;
        }

        for (int i = 0; i < missions.Count; i++)
        {
            if (i <= completedIndex)
            {
                missions[i].status = MissionStatus.Completed;
            }
            else if (i == completedIndex + 1)
            {
                missions[i].status = MissionStatus.Unlocked;
            }
            else
            {
                missions[i].status = MissionStatus.Locked;
            }
        }

        for (int i = 0; i < missions.Count; i++)
        {
            GameObject entryObj;
            MissionEntry entry;

            if (i == 0 && content.childCount > 0)
            {
                Transform firstChild = content.GetChild(0);
                entry = firstChild.GetComponent<MissionEntry>();
            }
            else
            {
                entryObj = Instantiate(missionEntryPrefab, content);
                entry = entryObj.GetComponent<MissionEntry>();
            }

            entry.Setup(missions[i].image, missions[i].title, missions[i].description, missions[i].sceneName, missions[i].status);
        }
    }

    public enum MissionStatus
    {
        Locked,
        Unlocked,
        Completed
    }
}
