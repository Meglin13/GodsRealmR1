using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocumentLocalization))]
    public class CaptionManager : UIScript
    {
        public static CaptionManager Instance;

        private float CaptionLifeTime;

        public ScrollView CaptionListSV;

        private void Awake()
        {
            Instance = this;
        }

        internal void OnEnable()
        {
            CaptionLifeTime = GameManager.Instance.CaptionLifeTime;

            //CaptionListSV = root.Q<ScrollView>();
        }

        public void ShowCaption(string CaptionText)
        {

        }
    }
}