using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class InventorySlotControl : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<InventorySlotControl, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        private Image Icon;
        private Label AmountLB;
        public Item itemContext;

        private StyleColor defaultBG;
        private StyleColor defaultBorderColor;

        public InventorySlotControl()
        {
            Icon = new Image();
            AmountLB = new Label();

            Add(AmountLB);
            Add(Icon);

            Icon.AddToClassList("slot-icon");
            AmountLB.AddToClassList("slot-amount-label");

            this.AddToClassList("slot");

            defaultBG = Icon.style.backgroundColor;
            defaultBorderColor = this.style.borderTopColor;
        }

        public void SetSlot(Item item)
        {
            if (item != null)
            {
                itemContext = item;

                AmountLB.text = item.Amount == 1 ? "" : item.Amount.ToString();

                Icon.sprite = item.Icon;

                Color color = GameManager.Instance.colorManager.RarityColor[item._Rarity];
                color.a = 0.8f;
                Icon.style.backgroundColor = color;
            }
            else
            {
                AmountLB.text = null;

                Icon.sprite = null;
                Icon.style.backgroundColor = defaultBG;

                itemContext = null;
            }
        }

        public void SelectSlot()
        {
            if (itemContext != null & ColorUtility.TryParseHtmlString("#FFC700", out Color color))
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