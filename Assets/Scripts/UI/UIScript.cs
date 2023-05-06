using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocumentLocalization))]
    public abstract class UIScript : MonoBehaviour
    {
        public VisualElement root;
        public Button BackBT;
        [HideInInspector]
        private UIDocumentLocalization localization;
        [HideInInspector]
        internal UIManager manager;

        internal StringTable UITable;
        internal StringTable ItemsTable;
        internal StringTable CharacterTable;
        internal StringTable DialogueTable;
        internal StringTable TutorialTable;

        private void OnEnable()
        {
            localization = GetComponent<UIDocumentLocalization>();
            manager = UIManager.Instance;

            UITable = manager.UITable;
            CharacterTable = manager.CharacterTable;
            ItemsTable = manager.ItemsTable;
            DialogueTable = manager.DialogueTable;
            TutorialTable = manager.TutorialTable;

            OnBind();
            localization.OnCompleted -= OnBind;
            localization.OnCompleted += OnBind;
        }

        internal virtual void OnBind()
        {
            root = GetComponent<UIDocument>().rootVisualElement;

            BackBT = root.Q<Button>("BackBT");

            if (BackBT != null)
            {
                BackBT.clicked -= GoBack;
                BackBT.clicked += GoBack;
            }
        }

        internal virtual void GoBack() => manager.GoBack();
    }
}