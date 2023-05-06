#if UNITY_EDITOR
using NaughtyAttributes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyBox;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using DialogueSystem;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.Google;

namespace Localization
{
    [CreateAssetMenu(fileName = "LocalizationManager", menuName = "Objects/TestingUtilities/LocalizationManager")]
    public class LocalizationManager : ScriptableObject
    {
        public List<ILocalizable> localizables;
        public List<Object> objects;

        [ButtonMethod]
        public void GetKeys()
        {
            localizables = ObjectsUtilities.FindAllScriptableObjects<ILocalizable>().ToList();
            objects = localizables.Cast<Object>().ToList();

            var chars = localizables.Where(x => x is EntityStats | x is Speaker).ToList();
            var items = localizables.Where(x => x is Item).ToList();
            var dialogues = localizables.Where(x => x is Dialogue).ToList();

            AddEntries(chars, LocalizationSettings.StringDatabase.GetTable("Entities"));
            AddEntries(items, LocalizationSettings.StringDatabase.GetTable("Items"));
            AddEntries(dialogues, LocalizationSettings.StringDatabase.GetTable("Dialogues"));
        }

        private void AddEntries(List<ILocalizable> localizables, StringTable table)
        {
            foreach (var local in localizables)
            {
                if (local is Dialogue dialogue)
                {
                    foreach (var replica in dialogue.replicas)
                    {
                        if (table.GetEntry(replica.Line) == null)
                            table.AddEntry(replica.Line, string.Empty);
                    }
                }
                else if (local is EntityStats stats)
                {
                    if (stats.Type == EntityType.Character)
                    {
                        foreach (var skill in stats.skills)
                            AddEntry(skill, table);
                    }
                }
                else
                {
                    AddEntry(local, table);
                }
            }
        }

        private void AddEntry(ILocalizable local, StringTable table)
        {
            if (local.Name != string.Empty | table.GetEntry(local.Name) == null)
                table.AddEntry(local.Name, string.Empty);

            if (local.Description != string.Empty | table.GetEntry(local.Description) == null)
                table.AddEntry(local.Description, string.Empty);
        }
    }
}
#endif