using UnityEngine;
using UnityEngine.Localization.Tables;
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

        internal virtual void OnBind()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
        }

        /// <summary>
        /// Установка нового текста и его локализация
        /// </summary>
        /// <param name="textElement">Текстовый элемент</param>
        /// <param name="key">Ключ в таблице локализации</param>
        internal void ChangeLabelsText(TextElement textElement, string key)
        {
            var table = localization._table.GetTable();

            if (!string.IsNullOrEmpty(key))
            {
                StringTableEntry entry = table[key];
                if (entry != null)
                    textElement.text = entry.LocalizedValue;
                else
                    textElement.text = key;
            }
        }

        internal virtual void GoBack() => UIManager.Instance.GoBack();
    }
}