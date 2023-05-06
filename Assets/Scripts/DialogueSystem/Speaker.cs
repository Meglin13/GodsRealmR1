using MyBox;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = "Speaker", menuName = "Objects/Dialogue System/Speaker")]
    public class Speaker : ScriptableObject, ILocalizable
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            this._Name = Path.GetFileNameWithoutExtension(assetPath);
        }
#endif
        [SerializeField]
        private string _Name;
        [Foldout("Sprites", true)]
        public Sprite Idle;

        public string Name => _Name;
        public string Description => string.Empty;
    }
}