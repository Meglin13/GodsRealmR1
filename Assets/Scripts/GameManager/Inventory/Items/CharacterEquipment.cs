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
        inventory = GameManager.Instance.inventory;

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

    public bool EquipItem(EquipmentItem item)
    {
        if (EquipmentSlots[item.EquipmentType] != null)
            UnequipItem(EquipmentSlots[item.EquipmentType]);

        EquipmentSlots[item.EquipmentType] = item;

        foreach (var modifier in item.Modifiers)
        {
            //TODO: Решить что делать со шмотками на слоты
            //if (modifier.StatType == StatType.InventorySlots)
            //    InventoryScript.Instance.AddCapacity(Mathf.FloorToInt(modifier.Amount));
            //else
            GameManager.Instance.StartCoroutine(character.AddModifier(modifier));
        }

        inventory.DeleteItem(item);

        return true;
    }

    public bool UnequipItem(EquipmentItem item)
    {
        if (inventory.Inventory.Count == inventory.Capacity)
            return false;

        EquipmentSlots[item.EquipmentType] = null;

        foreach (var modifier in item.Modifiers)
        {
            if (modifier.StatType == StatType.InventorySlots)
                InventoryScript.Instance.AddCapacity(Mathf.FloorToInt(-modifier.Amount));
            else
            {
                if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
                {
                    EntityStats.ElementSheet element = character.EntityStats.ElementsResBonus[modifier.Element];

                    if (modifier.StatType == StatType.Resistance)
                        element.Resistance.RemoveModifier(modifier);
                    else
                        element.DamageBonus.RemoveModifier(modifier);
                }
                else
                    character.EntityStats.ModifiableStats[modifier.StatType].RemoveModifier(modifier);
            }
        }

        inventory.AddItemToInventory(item, false);

        return true;
    }
}