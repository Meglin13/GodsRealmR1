using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace UI
{
    public class InventoryUIScript : UIScript
    {
        //Context
        public CharacterScript CharacterContext;

        public int CurrentCharIndex;
        public List<CharacterScript> Party;

        public InventoryScript inventory;
        public Item ItemContext;

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

        public VisualElement InventoryContainer;

        internal override void OnBind()
        {
            base.OnBind();

            CharacterContext = GameManager.GetInstance().partyManager.GetPlayer();
            CurrentCharIndex = GameManager.GetInstance().partyManager.LeaderIndex;
            Party = GameManager.GetInstance().partyManager.PartyMembers;
            inventory = GameManager.GetInstance().inventory;

            OnInventoryUpdate += () => UpdateInventory();
            OnInventoryUpdate += () => UpdateCharacterInfo();
            OnInventoryUpdate += () => SetInventoryCapacity();


            OnItemSelect += () => LoadItemInfo();

            InventoryCapacityLB = root.Q<Label>("InventoryCapacityLB");
            ItemInfoButtonsPanel = root.Q<VisualElement>("ItemInfoButtonsPanel");

            ItemInfoButtonsPanel.visible = false;

            //CharInfo
            CharNameLB = root.Q<Label>("CharNameLB");
            PreviousCharBT = root.Q<Button>("PreviousCharBT");
            NextCharBT = root.Q<Button>("NextCharBT");
            CharImage = root.Q<VisualElement>("CharImage");

            charSlots = new Dictionary<EquipmentType, InventorySlotControl>(8);

            for (int i = 0; i < 8; i++)
            {
                charSlots.Add((EquipmentType)i, root.Q<InventorySlotControl>((EquipmentType)i + "Slot"));
                var slot = charSlots.ElementAt(i).Value;
                slot.AddManipulator(new Clickable(evt => SelectItem(slot.slotContext)));
            }

            //ItemInfo
            EquipBT = root.Q<Button>("EquipBT");
            DropBT = root.Q<Button>("DropBT");

            ItemNameLB = root.Q<Label>("ItemNameLB");
            ItemDescLB = root.Q<Label>("ItemDescLB");

            //Inventory
            InventoryContainer = root.Q<VisualElement>("InventoryContainer");

            //Button events
            EquipBT.clicked += EquipItemButtonClicked;
            DropBT.clicked += DropItemButtonClicked;

            PreviousCharBT.clicked += () => ChangeCharacterContext(IndexType.Previous);
            NextCharBT.clicked += () => ChangeCharacterContext(IndexType.Next);

            OnInventoryUpdate();
        }

        private void UpdateInventory()
        {
            InventoryContainer.Clear();

            for (int i = 0; i < inventory.Capacity; i++)
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

                        inventorySlot.AddManipulator(new Clickable(evt => SelectItem(inventorySlot.slotContext)));
                    }
                }

                InventoryContainer.Add(inventorySlot);
            }
        }

        private void UpdateCharacterInfo()
        {
            CharNameLB.text = "#" + CharacterContext.EntityStats.Name;
            CharImage.style.backgroundImage = new StyleBackground(CharacterContext.EntityStats.Art);

            //TODO: Оптимизировать

            for (int i = 0; i < 8; i++)
            {
                charSlots[(EquipmentType)i].SetSlot(CharacterContext.equipment.EquipmentSlots[(EquipmentType)i]);
            }
        }

        private void SelectItem(Item item)
        {
            if (item)
            {
                if (!ItemInfoButtonsPanel.visible)
                {
                    ItemInfoButtonsPanel.visible = true;
                }

                EquipBT.text = IsItemEquiped(item) ? "#Unequip" : "#Equip";

                ItemContext = item;

                OnItemSelect();
            }
        }

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
            if (IsItemEquiped(ItemContext))
            {
                if (inventory.Inventory.Count < inventory.Inventory.Capacity)
                {
                    CharacterContext.equipment.UnequipItem(ItemContext as EquipmentItem);
                }
            }
            else
            {
                CharacterContext.equipment.EquipItem(ItemContext as EquipmentItem);
            }

            OnInventoryUpdate();
        }

        private void DropItemButtonClicked()
        {
            throw new NotImplementedException();
        }

        private void LoadItemInfo()
        {
            if (ItemContext)
            {
                ItemNameLB.text = "#" + ItemContext.Name;
                ItemDescLB.text = "#" + ItemContext.Description;
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

        private void SetInventoryCapacity()
        {
            InventoryCapacityLB.text = $"{inventory.Inventory.Count} / {inventory.Inventory.Capacity}";
        }
    }
}