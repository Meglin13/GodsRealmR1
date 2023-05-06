using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class CharacterSlot : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<CharacterSlot, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        public EntityStats entityStats;

        private StyleColor defaultBorderColor;
        private Image Icon;

        public CharacterSlot()
        {
            AddToClassList("slot");

            Icon = new Image();
            Icon.AddToClassList("slot-icon");
            Add(Icon);

            style.height = 150;
            style.width = 150;

            defaultBorderColor = this.style.borderTopColor;
        }

        public void SetSlot(EntityStats entityStats)
        {
            if (entityStats != null)
            {
                this.entityStats = entityStats;

                Icon.sprite = entityStats.Icon;

                Color color = GameManager.Instance.colorManager.RarityColor[entityStats.Rarity];
                color.a = 0.8f;

                style.backgroundColor = color;
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
