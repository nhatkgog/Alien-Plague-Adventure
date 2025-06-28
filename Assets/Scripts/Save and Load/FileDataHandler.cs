using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Save_and_Load
{
    public class FileDataHandler
    {
        private string dataDirPath = "";
        private string dataFileName = "";

        public FileDataHandler(string dataDirPath, string dataFileName)
        {
            this.dataDirPath = dataDirPath;
            this.dataFileName = dataFileName;
        }
        public void Save(GameData gameData)
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string dataToStore = JsonUtility.ToJson(gameData, true);

                Debug.Log("Saving data to: " + fullPath);
                Debug.Log("Data: " + dataToStore);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(dataToStore);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save data: " + e.Message);
            }
        }


        public GameData Load()
        {
            string fullPath = Path.Combine(dataDirPath, dataFileName);
            GameData loadData = null;

            if (File.Exists(fullPath))
            {
                try
                {
                    string dataToLoad = "";
                    using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToLoad = reader.ReadToEnd();

                        }
                    }
                    loadData = JsonUtility.FromJson<GameData>(dataToLoad);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to load data: " + e.Message);
                }


            }
            return loadData;
        }
    }
}
