using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocumentLocalization))]
    public abstract class UIScript : MonoBehaviour
    {
        public VisualElement root;
        [HideInInspector]
        private UIDocumentLocalization localization;
        [HideInInspector]
        public UIManager manager;

        internal StringTable UITable;
        internal StringTable ItemsTable;
        internal StringTable CharacterTable;

        private void OnEnable()
        {
            localization = GetComponent<UIDocumentLocalization>();
            manager = UIManager.Instance;

            UITable = manager.UITable;
            CharacterTable = manager.CharacterTable;
            ItemsTable = manager.ItemsTable;

            OnBind();
            localization.OnCompleted -= OnBind;
            localization.OnCompleted += OnBind;
        }

        internal virtual void OnBind()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            var BackBT = root.Q<Button>("BackBT");
            if (BackBT != null)
            {
                BackBT.clicked += GoBack;
            }
        }

        internal virtual void GoBack() => manager.GoBack();
    }
}