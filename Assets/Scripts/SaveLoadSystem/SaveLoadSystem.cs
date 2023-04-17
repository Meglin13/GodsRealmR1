using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UI;
using UnityEngine;

namespace SaveLoadSystem
{
    //TODO: Система сохранения
    [Serializable]
    public class GameData
    {
        public GameData(int tokens, List<int> unlockedCharactersId)
        {
            this.Tokens = tokens;
            this.UnlockedCharactersId = unlockedCharactersId;
        }

        public int Tokens { get; set; }
        public List<int> UnlockedCharactersId { get; set; }
    }

    public static class SaveLoadSystem
    {
        public static string savingPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\God's Realm\Saved Game";

        private static GameData CollectGameData()
        {
            var unlocked = new List<int>();
            var list =  PartyManager.Instance.Characters.Where(x => x.IsUnlocked);

            foreach (var item in list)
            {
                unlocked.Add(item.ID);
            }

            GameData result = new GameData(0, unlocked);

            return result;
        }

        public static void Save()
        {
            var data = CollectGameData();

            if (!Directory.Exists(savingPath))
                Directory.CreateDirectory(savingPath);

            try
            {
                string path = savingPath + @"\GameData.dat";

                BinaryFormatter binaryFormatter = new BinaryFormatter();

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    binaryFormatter.Serialize(stream, data);
                }

                UnityEngine.Debug.Log("Saved");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Something went wrong.\n" + ex.Message);
            }
        }

        public static void Load()
        {
            GameData state;

            string path = savingPath + @"\GameData.dat";

            if (!File.Exists(path))
            {
                UnityEngine.Debug.Log("No saved games");
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                state = (GameData)formatter.Deserialize(stream);

                var list = Resources.LoadAll("ScriptableObjects/Character", typeof(EntityStats)).Cast<EntityStats>().ToList();

                for (int i = 0; i < list.Count; i++)
                {
                    if (state.UnlockedCharactersId.Contains(list[i].ID))
                    {
                        list[i].IsUnlocked = true;
                    }
                }
            }
        }

        public static void DeleteSave()
        {
            string path = savingPath + @"\GameData.dat";
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                    UnityEngine.Debug.Log("Deleted");
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.Log("Something went wrong.\n" + ex.Message);
                }
            }

            var list = Resources.LoadAll("ScriptableObjects/Character", typeof(EntityStats)).Cast<EntityStats>().ToList();
            
            foreach (var entity in list)
            {
                if (entity.Name != "Ardalion" | entity.Name != "Marfa" | entity.Name != "Dream")
                {
                    entity.IsUnlocked = false;
                }
            }
        }
    }
}