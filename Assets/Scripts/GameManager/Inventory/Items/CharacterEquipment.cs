using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class CharacterEquipment
{
    CharacterScript character;
    InventoryScript inventory;

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
    private EquipmentItem Helmet;
    [SerializeField]
    private EquipmentItem Armor;
    [SerializeField]
    private EquipmentItem Boots;
    [SerializeField]
    private EquipmentItem Gloves;

    [SerializeField]
    private EquipmentItem Ring;
    [SerializeField]
    private EquipmentItem Bracelet;
    [SerializeField]
    private EquipmentItem Amulet;
    [SerializeField]
    private EquipmentItem Artefact;

    public void EquipItem(EquipmentItem item)
    {
        if (item)
        {
            if (EquipmentSlots[item.EquipmentType] != null)
            {
                UnequipItem(EquipmentSlots[item.EquipmentType]);
            }

            EquipmentSlots[item.EquipmentType] = item;

            foreach (var ad in item.Modifiers)
            {
                character.EntityStats.ModifiableStats[ad.StatType].AddModifier(ad);
            }

            inventory.DeleteItem(item);
        }
    }

    public void UnequipItem(EquipmentItem item)
    {
        if (item)
        {
            EquipmentSlots[item.EquipmentType] = null;

            foreach (var ad in item.Modifiers)
            {
                character.EntityStats.ModifiableStats[ad.StatType].RemoveModifier(ad);
            }

            inventory.AddItemToInventory(item);
        }
    }
}
