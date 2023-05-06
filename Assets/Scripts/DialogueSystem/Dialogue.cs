using MyBox;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogueSystem
{
    [Serializable]
    public class Replica
    {
#if UNITY_EDITOR
        [SerializeField]
        private string Comment;
        [HideInInspector]
        public Speaker[] speakers;
#endif
        [Dropdown("speakers")]
        public Speaker Speaker;
        [MyBox.ReadOnly]
        public string Line;
    }

    [CreateAssetMenu(fileName = "Dialogue", menuName = "Objects/Dialogue System/Dialogue")]
    public class Dialogue : ScriptableObject, ILocalizable, ICollectable
    {
#if UNITY_EDITOR
        [ButtonMethod]
        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            _name = Path.GetFileNameWithoutExtension(assetPath);

            foreach (var item in replicas)
            {
                if (item.Speaker != null)
                    item.Line = $"{_name}_{item.Speaker.Name}_{replicas.IndexOf(item) + 1}";
            }
        }

#endif

        [Button]
        public void UpdateSpeakers()
        {
            foreach (var item in replicas)
                item.speakers = Speakers;
        }

        [SerializeField]
        private string _name;
        public string Name => _name;
        public string Description => string.Empty;

        [SerializeField]
        [MyBox.ReadOnly]
        private bool unlocked = false;
        public bool IsUnlocked { get => unlocked; set => unlocked = value; }

        public Speaker[] Speakers;
        public List<Replica> replicas = new List<Replica>();
    }
}