using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterEquipment
{
    private CharacterScript character;
    private InventoryScript inventory;

    public CharacterEquipment(CharacterScript character)
    {
        this.character = character;
        inventory = GameManager.GetInstance().inventory;

        EquipmentSlots = new Dictionary<EquipmentType, EquipmentItem>()
        {
            { EquipmentType.Helmet, Helmet },
            { EquipmentType.Armor, Armor },
            { EquipmentType.Gloves, Gloves },
            { EquipmentType.Boots, Boots },
            { EquipmentType.Ring, Ring },
            { EquipmentType.Bracelet, Bracelet },
            { EquipmentType.Amulet, Amulet },
            { EquipmentType.Artefact, Artefact },
        };
    }

    public Dictionary<EquipmentType, EquipmentItem> EquipmentSlots;

    [SerializeField]
    private EquipmentItem Helmet, Armor, Boots, Gloves;

    [SerializeField]
    private EquipmentItem Ring, Bracelet, Amulet, Artefact;

    public void EquipItem(EquipmentItem item)
    {
        if (item)
        {
            if (EquipmentSlots[item.EquipmentType] != null)
            {
                if (inventory.Inventory.Count == inventory.Inventory.Capacity)
                {
                    return;
                }

                UnequipItem(EquipmentSlots[item.EquipmentType]);
            }

            item.IsEquiped = true;

            EquipmentSlots[item.EquipmentType] = item;

            foreach (var ad in item.Modifiers)
            {
                character.EntityStats.ModifiableStats[ad.StatType].AddModifier(ad);
            }

            inventory.DeleteItem(item.ID);
        }
    }

    public void UnequipItem(EquipmentItem item)
    {
        EquipmentSlots[item.EquipmentType] = null;

        item.IsEquiped = false;

        foreach (var ad in item.Modifiers)
        {
            character.EntityStats.ModifiableStats[ad.StatType].RemoveModifier(ad);
        }

        inventory.AddItemToInventory(item);
    }
}