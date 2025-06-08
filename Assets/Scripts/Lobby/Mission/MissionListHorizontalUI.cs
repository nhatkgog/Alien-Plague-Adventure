using System.Collections.Generic;
using UnityEngine;

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
    }

    public List<MissionData> missions;

    void Start()
    {
        foreach (var mission in missions)
        {
            GameObject entryObj = Instantiate(missionEntryPrefab, content);
            MissionEntry entry = entryObj.GetComponent<MissionEntry>();
            entry.Setup(mission.image, mission.title, mission.description, mission.sceneName);
        }
    }
}
