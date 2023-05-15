using Akaal.PvCustomizer.Scripts;
using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Tutorial
{
    [CreateAssetMenu(fileName = "InfoSheet", menuName = "Objects/TextInfo/InfoSheet")]
    public class InfoSheet : ScriptableObject, ILocalizable
    {
#if UNITY_EDITOR
        private void OnValidate()
        {
            _Description = _Name + "_Desc";
        }

        [SerializeField]
        private string Comment;
#endif
        [SerializeField]
        private string _Name;
        public string Name => _Name;
        [SerializeField]
        [ReadOnly]
        private string _Description;
        public string Description => _Description;

        public Sprite Image;
        [PvIcon]
        public Sprite Icon;

        public int Order = 0;
    }
}