using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SaveLoadSystem
{
    [Serializable]
    public class GameData
    {
        public GameData() { }

        public GameData(int tokens, List<int> unlockedCharactersId)
        {
            this.tokens = tokens;
            this.unlockedCharactersId = unlockedCharactersId;
        }

        [SerializeField] private int tokens;
        public int Tokens { get { return tokens; } }

        [SerializeField] private List<int> unlockedCharactersId;
        public List<int> UnlockedCharactersId { get { return unlockedCharactersId; } }
    }

    public static class SaveLoadSystem
    {
        private static GameData save;
        public static GameData SavedData
        {
            get => save ??= Load();
        }

        public static string savingPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\My Games\God's Realm\Saved Game";

        public static GameData CollectGameData()
        {
            var unlocked = new List<int>();
            var list = Resources.LoadAll("ScriptableObjects/Character", typeof(EntityStats)).Cast<EntityStats>().ToList().Where(x => x.IsUnlocked);

            foreach (var item in list)
            {
                unlocked.Add(item.ID);
            }

            GameData result = new GameData(0, unlocked);

            return result;
        }

        public static bool Save()
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
                return true;
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Something went wrong.\n" + ex.Message);
                return false;
            }
        }

        public static GameData Load()
        {
            GameData state;

            string path = savingPath + @"\GameData.dat";

            if (!File.Exists(path))
            {
                UnityEngine.Debug.Log("No saved games");
                return null;
            }

            try
            {
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

                UnityEngine.Debug.Log("Loaded");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log("Something went wrong.\n" + ex.Message);
                return null;
            }

            return state;
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
                    entity.IsUnlocked = false;
            }

            var collectebles = ObjectsUtilities.FindAllScriptableObjects<ICollectable>();

            foreach (var item in collectebles)
                item.IsUnlocked = false;
        }
    }
}