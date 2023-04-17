using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UI.CustomControls;

namespace UI
{
    public class InventoryUIScript : UIScript
    {

        //Context
        public CharacterScript CharacterContext;

        public int CurrentCharIndex;
        public List<CharacterScript> Party;

        public InventoryScript inventory;
        public InventorySlotControl slotContext;
        private int SelectedSlotIndex = 0;

        public enum IndexType
        { Next, Previous }

        public event Action OnInventoryUpdate = delegate { };

        public event Action OnItemSelect = delegate { };

        //Inventory
        public Label InventoryCapacityLB;

        public VisualElement ItemInfoButtonsPanel;

        //Character menu
        public Label CharNameLB;

        public VisualElement CharImage;

        public Button PreviousCharBT;
        public Button NextCharBT;

        //EquipmentSlots
        public Dictionary<EquipmentType, InventorySlotControl> charSlots;

        public InventorySlotControl HelmetSlot;
        public InventorySlotControl ArmorSlot;
        public InventorySlotControl GlovesSlot;
        public InventorySlotControl BootsSlot;
        public InventorySlotControl BraceletSlot;
        public InventorySlotControl RingSlot;
        public InventorySlotControl AmuletSlot;
        public InventorySlotControl ArtefactSlot;

        //Item menu
        public Button EquipBT;

        public Button DropBT;

        public Label ItemNameLB;
        public Label ItemDescLB;

        public Label GoldLB;
        public Label TokenLB;

        public VisualElement InventoryContainer;

        internal override void OnBind()
        {
            base.OnBind();

            CharacterContext = GameManager.Instance.partyManager.GetPlayer();
            CurrentCharIndex = GameManager.Instance.partyManager.LeaderIndex;
            Party = GameManager.Instance.partyManager.PartyMembers;
            inventory = GameManager.Instance.inventory;

            OnInventoryUpdate += () => UpdateInventory();
            OnInventoryUpdate += () => UpdateCharacterInfo();
            OnInventoryUpdate += () => SetInventoryCapacity();
            OnInventoryUpdate += () => SetCurrencies();

            OnItemSelect += () => LoadItemInfo();

            InventoryCapacityLB = root.Q<Label>("InventoryCapacityLB");
            ItemInfoButtonsPanel = root.Q<VisualElement>("ItemInfoButtonsPanel");

            ItemInfoButtonsPanel.visible = false;

            //CharInfo
            CharNameLB = root.Q<Label>("CharNameLB");
            PreviousCharBT = root.Q<Button>("PreviousBT");
            NextCharBT = root.Q<Button>("NextBT");
            CharImage = root.Q<VisualElement>("CharImage");

            charSlots = new Dictionary<EquipmentType, InventorySlotControl>(8);

            for (int i = 0; i < 8; i++)
            {
                charSlots.Add((EquipmentType)i, root.Q<InventorySlotControl>((EquipmentType)i + "Slot"));
                var slot = charSlots.ElementAt(i).Value;
                slot.AddManipulator(new Clickable(evt => SelectItemSlot(slot)));
            }

            //ItemInfo
            EquipBT = root.Q<Button>("EquipBT");
            DropBT = root.Q<Button>("DropBT");

            ItemNameLB = root.Q<Label>("ItemNameLB");

            ItemDescLB = root.Q<Label>("ItemDescLB");

            GoldLB = root.Q<Label>("GoldLB");
            TokenLB = root.Q<Label>("TokenLB");

            //Inventory
            InventoryContainer = root.Q<VisualElement>("InventoryContainer");

            //Button events
            EquipBT.clicked += EquipItemButtonClicked;
            DropBT.clicked += DropItemButtonClicked;

            PreviousCharBT.clicked += () => ChangeCharacterContext(IndexType.Previous);
            NextCharBT.clicked += () => ChangeCharacterContext(IndexType.Next);

            OnInventoryUpdate();
        }

        private void OnDisable()
        {
            EquipBT.clicked -= EquipItemButtonClicked;
            DropBT.clicked -= DropItemButtonClicked;

            PreviousCharBT.clicked -= () => ChangeCharacterContext(IndexType.Previous);
            NextCharBT.clicked -= () => ChangeCharacterContext(IndexType.Next);
        }

        private void UpdateInventory()
        {
            InventoryContainer.Clear();

            for (int i = 0; i < inventory.Inventory.Capacity; i++)
            {
                Item item = null;

                InventorySlotControl inventorySlot = new InventorySlotControl();

                if (inventory.Inventory.Count != 0)
                {
                    if (i < inventory.Inventory.Count)
                    {
                        item = Instantiate(inventory.Inventory[i]);
                        item.name = inventory.Inventory[i].name;

                        inventorySlot.SetSlot(item);

                        inventorySlot.AddManipulator(new Clickable(evt => SelectItemSlot(inventorySlot)));
                    }
                }

                InventoryContainer.Add(inventorySlot);
            }

            if (inventory.Inventory.Count == 0)
            {
                ItemInfoButtonsPanel.visible = false;
                ItemDescLB.text = string.Empty;
                ItemNameLB.text = string.Empty;
            }
        }

        #region [Equipment Managment]

        private bool IsItemEquiped(Item item)
        {
            if (item is EquipmentItem)
            {
                EquipmentItem equipment = item as EquipmentItem;
                return equipment.IsEquiped;
            }
            return false;
        }

        private void EquipItemButtonClicked()
        {
            slotContext.itemContext.UseItem(CharacterContext);

            if (inventory.Inventory.Count < inventory.Inventory.Capacity)
                SelectedSlotIndex = inventory.Inventory.Count - 1;

            OnInventoryUpdate();

            ChangeFocus();
        }

        private void DropItemButtonClicked()
        {
            if (slotContext.itemContext)
            {
                Item item = slotContext.itemContext;

                SelectedSlotIndex = InventoryContainer.IndexOf(slotContext);

                if (IsItemEquiped(item))
                {
                    CharacterContext.equipment.UnequipItem(item as EquipmentItem);
                    SelectedSlotIndex = inventory.Inventory.Count - 1;
                }

                inventory.DeleteItem(item.ID);

                OnInventoryUpdate();

                if (inventory.Inventory.Count > 0)
                {
                    ChangeFocus();
                }
            }
        }

        #endregion [Equipment Managment]

        #region [Context]

        private void SelectItemSlot(InventorySlotControl inventorySlot)
        {
            if (inventorySlot.itemContext)
            {
                if (!ItemInfoButtonsPanel.visible)
                    ItemInfoButtonsPanel.visible = true;

                if (inventorySlot.itemContext is EquipmentItem)
                {
                    manager.ChangeLabelsText(EquipBT, IsItemEquiped(inventorySlot.itemContext) ? "Unequip" : "Equip", UITable);
                }
                else
                {
                    manager.ChangeLabelsText(EquipBT, "Use", UITable);
                }

                slotContext?.UnselectSlot();
                inventorySlot.SelectSlot();

                SelectedSlotIndex = InventoryContainer.IndexOf(inventorySlot);

                slotContext = inventorySlot;

                OnItemSelect();
            }
        }

        private void ChangeCharacterContext(IndexType indexType)
        {
            if (indexType == IndexType.Next)
            {
                MiscUtilities.Instance.NextIndex(Party, ref CurrentCharIndex);
            }
            else
            {
                MiscUtilities.Instance.PreviousIndex(Party, ref CurrentCharIndex);
            }

            CharacterContext = Party[CurrentCharIndex];

            OnInventoryUpdate();
        }

        private void ChangeFocus()
        {
            int index = SelectedSlotIndex;

            if (index > inventory.Inventory.Count - 1)
            {
                index = inventory.Inventory.Count - 1;
            }
            else
            {
                return;
            }

            SelectItemSlot((InventorySlotControl)InventoryContainer[index]);
        }

        #endregion [Context]

        #region [Loading Info]

        private void SetInventoryCapacity()
        {
            InventoryCapacityLB.text = $"{inventory.Inventory.Count} / {inventory.Inventory.Capacity}";
        }

        private void SetCurrencies()
        {
            manager.ChangeLabelsText(GoldLB, "Gold", UITable);
            GoldLB.text += " " + inventory.Gold;

            manager.ChangeLabelsText(TokenLB, "Token", UITable);
            TokenLB.text += " " + inventory.Tokens;
        }

        private void UpdateCharacterInfo()
        {
            manager.ChangeLabelsText(CharNameLB, CharacterContext.EntityStats.Name, CharacterTable);

            CharImage.style.backgroundImage = new StyleBackground(CharacterContext.EntityStats.Art);

            for (int i = 0; i < 8; i++)
            {
                charSlots[(EquipmentType)i].SetSlot(CharacterContext.equipment.EquipmentSlots[(EquipmentType)i]);
            }
        }

        private void LoadItemInfo()
        {
            if (slotContext.itemContext)
            {
                manager.ChangeLabelsText(ItemNameLB, slotContext.itemContext.Name, ItemsTable);
                manager.ChangeLabelsText(ItemDescLB, slotContext.itemContext.Description, ItemsTable);
            }
        }

        #endregion [Loading Info]
    }
}