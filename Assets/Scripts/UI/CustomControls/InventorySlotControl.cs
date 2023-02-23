using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI
{
    public class InventorySlotControl : VisualElement
    {
        #region UXML
        [Preserve]
        public new class UxmlFactory : UxmlFactory<InventorySlotControl, UxmlTraits> { }
        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits { }
        #endregion

        public Image Icon;
        public Label AmountLB;
        public Item slotContext;

        public StyleColor defaultBG;

        public InventorySlotControl()
        {
            Icon = new Image();
            AmountLB = new Label();

            Add(AmountLB);
            Add(Icon);
           
            Icon.AddToClassList("slot-icon");
            AmountLB.AddToClassList("slot-amount-label");

            AddToClassList("inventory-slot");

            defaultBG = Icon.style.backgroundColor;
        }

        public void SetSlot(Item item)
        {
            if (item != null)
            {
                AmountLB.text = item.Amount == 1 ? "" : item.Amount.ToString();

                Icon.sprite = item.Icon;
                Icon.style.backgroundColor = GameManager.Instance.colorManager.RarityColor[item.Rarity];

                slotContext = item;
            }
            else
            {
                AmountLB.text = null;

                Icon.sprite = null;
                Icon.style.backgroundColor = defaultBG;

                slotContext = null;
            }
        }
    }
}