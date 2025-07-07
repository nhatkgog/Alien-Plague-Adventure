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

        for (int i = 0; i < missions.Count; i++)
        {
            GameObject entryObj;
            MissionEntry entry;

            if (i == 0)
            {
                Transform firstChild = content.GetChild(0);
                entry = firstChild.GetComponent<MissionEntry>();
            }
            else
            {
                entryObj = Instantiate(missionEntryPrefab, content);
                entry = entryObj.GetComponent<MissionEntry>();
            }

            entry.Setup(missions[i].image, missions[i].title, missions[i].description, missions[i].sceneName);

        }
    }

}
