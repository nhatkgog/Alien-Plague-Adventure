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
        if (missions.Count == 0) return;

        Transform firstChild = content.GetChild(0);
        MissionEntry entry0 = firstChild.GetComponent<MissionEntry>();
        entry0.Setup(missions[0].image, missions[0].title, missions[0].description, missions[0].sceneName);

        for (int i = 1; i < missions.Count; i++)
        {
            GameObject entryObj = Instantiate(missionEntryPrefab, content);
            MissionEntry entry = entryObj.GetComponent<MissionEntry>();
            entry.Setup(missions[i].image, missions[i].title, missions[i].description, missions[i].sceneName);
        }
    }
}
