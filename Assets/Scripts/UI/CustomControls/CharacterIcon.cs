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

        public EntityStats entityStats;

        private Image ElementIcon;
        private Label CharName;
        private VisualElement LabelContainer;

        public Button DeleteButton;

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

            DeleteButton = new Button();
            DeleteButton.AddToClassList("char-slot-delete-button");
            DeleteButton.RemoveFromClassList("unity-button");
            DeleteButton.RemoveFromClassList("Button");
            DeleteButton.text = "#Delete";
            LabelContainer.Add(DeleteButton);

            defaultBorderColor = this.style.borderTopColor;

            DeleteButton.clicked += ClearSlot;
        }

        private void ClearSlot() => SetSlot(null);

        public void SetSlot(EntityStats entityStats)
        {
            if (entityStats != null)
            {
                this.entityStats = entityStats;

                this.style.backgroundImage = new StyleBackground(entityStats.Art);

                UIManager.Instance.ChangeLabelsText(CharName, entityStats.Name, UIManager.Instance.CharacterTable);

                ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[entityStats.Element], out Color color);
                ElementIcon.style.backgroundColor = new StyleColor(color);

                DeleteButton.style.visibility = Visibility.Visible;
            }
            else
            {
                this.style.backgroundImage = null;
                CharName.text = string.Empty;

                DeleteButton.style.visibility = Visibility.Hidden;
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