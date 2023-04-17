using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class CharacterIcon : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<CharacterIcon, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        private EntityStats entityStats;

        private Image ElementIcon;
        private Label CharName;
        private VisualElement LabelContainer;

        private StyleColor defaultBorderColor;

        public CharacterIcon()
        {
            ElementIcon = new Image();
            Add(ElementIcon);
            ElementIcon.AddToClassList("element-icon");

            LabelContainer = new VisualElement();
            LabelContainer.AddToClassList("label-gradient-container");
            Add(LabelContainer);

            CharName = new Label();
            CharName.text = "PLACEHOLDER";
            LabelContainer.Add(CharName);
            CharName.AddToClassList("charname-label");

            AddToClassList("char-slot");

            defaultBorderColor = this.style.borderTopColor;
        }

        public void SetSlot(EntityStats entityStats)
        {
            if (entityStats != null)
            {
                this.entityStats = entityStats;

                this.style.backgroundImage = new StyleBackground(entityStats.Art);
                CharName.text = entityStats.Name;

                ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[entityStats.Element], out Color color);
                ElementIcon.style.backgroundColor = new StyleColor(color);
            }
            else
            {
                this.style.backgroundImage = null;
                CharName.text = null;
            }
        }

        public void SelectSlot()
        {
            if (entityStats != null & ColorUtility.TryParseHtmlString("#FFC700", out Color color))
            {
                SetBorderColor(color);
            }
        }

        public void UnselectSlot()
        {
            SetBorderColor(defaultBorderColor);
        }

        public void SetBorderColor(StyleColor color)
        {
            style.borderRightColor = color;
            style.borderLeftColor = color;
            style.borderTopColor = color;
            style.borderBottomColor = color;
        }
    }
}