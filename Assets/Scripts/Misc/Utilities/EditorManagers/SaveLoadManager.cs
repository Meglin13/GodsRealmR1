using MyBox;
using SaveLoadSystem;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "SaveManager", menuName = "Objects/TestingUtilities/SaveManager")]
public class SaveLoadManager : ScriptableObject
{
    public GameData gameData;
    public GameData SavedData;

#if UNITY_EDITOR

    public void OnValidate()
    {
        gameData = SaveLoadSystem.SaveLoadSystem.CollectGameData();
    }

#endif

    [ButtonMethod]
    public void OpenSaveFile()
    {
        Process.Start(SaveLoadSystem.SaveLoadSystem.savingPath);
    }

    [ButtonMethod]
    public void Save()
    {
        SaveLoadSystem.SaveLoadSystem.Save();
    }

    [ButtonMethod]
    public void Load()
    {
        SavedData = SaveLoadSystem.SaveLoadSystem.Load();
    }

    [ButtonMethod]
    public void DeleteSave()
    {
        SaveLoadSystem.SaveLoadSystem.DeleteSave();
    }
}