using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public abstract class UIScript : MonoBehaviour
    {
        public VisualElement root;
        [HideInInspector]
        public UIDocumentLocalization localization;

        private void OnEnable()
        {
            localization = GetComponent<UIDocumentLocalization>();

            OnBind();
            localization.OnCompleted -= OnBind;
            localization.OnCompleted += OnBind;
        }

        //TODO: Обновление локализации
        internal virtual void OnBind()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }

        internal virtual void GoBack() => UIManager.Instance.GoBack();
    }
}