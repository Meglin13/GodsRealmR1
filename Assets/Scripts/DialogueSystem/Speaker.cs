using MyBox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    public enum DialogueSpriteType
    {
        Idle, Sad, Thinking, Angry, Happy
    }

    [Serializable]
    public class SpeakerSprite
    {
#if UNITY_EDITOR
        [HideInInspector]
        public string Name;
#endif
        [ReadOnly]
        public DialogueSpriteType Mood;
        public Sprite Sprite;

        public SpeakerSprite(DialogueSpriteType mood)
        {
            Mood = mood;
#if UNITY_EDITOR
            Name = mood.ToString();
#endif
        }
    }

    [CreateAssetMenu(fileName = "Speaker", menuName = "Objects/Dialogue System/Speaker")]
    public class Speaker : ScriptableObject, ILocalizable
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            this._Name = Path.GetFileNameWithoutExtension(assetPath);

            for (int i = 0; i < Enum.GetNames(typeof(DialogueSpriteType)).Length; i++)
            {
                if (Sprites.Where(x => x.Mood == (DialogueSpriteType)i).ToList().Count == 0)
                {
                    Sprites.Add(new SpeakerSprite((DialogueSpriteType)i));
                }
            }
        }
#endif
        [SerializeField]
        private string _Name;
        public Sprite Idle;
        public List<SpeakerSprite> Sprites;

        public string Name => _Name;
        public string Description => string.Empty;
    }
}