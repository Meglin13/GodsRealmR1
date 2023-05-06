using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocumentLocalization))]
    public class CaptionManager : UIScript
    {
        public static CaptionManager Instance;

        private float CaptionLifeTime = 5f;

        public ScrollView CaptionListSV;

        private void Awake()
        {
            Instance = this;
        }

        internal void OnEnable()
        {

            //CaptionListSV = root.Q<ScrollView>();
        }
        //TODO: Менеджер уведомлений
        private IEnumerator ShowCaption(string CaptionText)
        {
            yield return new WaitForSeconds(CaptionLifeTime);
        }
    }
}