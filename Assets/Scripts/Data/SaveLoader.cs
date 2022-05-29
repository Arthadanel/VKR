using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
    public static class SaveLoader
    {
        public static void SaveFile(int saveSlot)
        {
            string destination = Application.persistentDataPath + "/save" + saveSlot + ".dat";

            FileStream file = File.Exists(destination) ? File.OpenWrite(destination) : File.Create(destination);

            SaveData data = new SaveData(SaveDataStorage.SEED, SaveDataStorage.IS_DIFFICULT,
                SaveDataStorage.OPEN_LEVELS,
                SaveDataStorage.LEVEL_COMPLETION);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }

        public static bool LoadFile(int saveSlot)
        {
            string destination = Application.persistentDataPath + "/save" + saveSlot + ".dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                Debug.LogError("File not found");
                return false;
            }

            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();

            SaveDataStorage.SEED = data.seed;
            SaveDataStorage.IS_DIFFICULT = data.isDifficult;
            SaveDataStorage.OPEN_LEVELS = new HashSet<int>(data.openLevels);
            SaveDataStorage.LEVEL_COMPLETION = data.levelCompletion;

            return true;
        }

        public static void DeleteFile(int saveSlot)
        {
            string destination = Application.persistentDataPath + "/save" + saveSlot + ".dat";
            FileStream file;
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
        }

        public static SaveData GetFileData(int saveSlot)
        {
            string destination = Application.persistentDataPath + "/save" + saveSlot + ".dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenRead(destination);
            else
            {
                return null;
            }

            BinaryFormatter bf = new BinaryFormatter();
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();

            return data;
        }
    }
}