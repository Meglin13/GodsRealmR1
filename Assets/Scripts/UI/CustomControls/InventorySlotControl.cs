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

        public Image Icon;
        public Label AmountLB;
        public Item itemContext;

        private StyleColor defaultBG;
        private StyleColor defaultBorderColor;

        public InventorySlotControl()
        {
            Icon = new Image();
            AmountLB = new Label();

            Add(Icon);
            Add(AmountLB);

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

                AmountLB.text = item.Amount.ToString();
                AmountLB.style.visibility = item.Amount == 1 ? Visibility.Hidden : Visibility.Visible;

                Icon.sprite = item.Icon;

                Color color = GameManager.Instance.colorManager.RarityColor[item._Rarity];
                color.a = 0.7f;
                this.style.backgroundColor = color;
            }
            else
            {
                AmountLB.style.visibility = Visibility.Hidden;

                Icon.sprite = null;
                this.style.backgroundColor = defaultBG;

                itemContext = null;
            }
        }

        public void SelectSlot()
        {
            if (itemContext != null & ColorUtility.TryParseHtmlString("#FFC700", out Color color))
                SetBorderColor(color);
        }

        public void UnselectSlot() => SetBorderColor(defaultBorderColor);

        private void SetBorderColor(StyleColor color)
        {
            style.borderRightColor = color;
            style.borderLeftColor = color;
            style.borderTopColor = color;
            style.borderBottomColor = color;
        }
    }
}