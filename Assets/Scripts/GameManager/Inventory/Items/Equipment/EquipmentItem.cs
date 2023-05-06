using MyBox;
using System;
using UnityEngine;

public enum EquipmentType
{
    Helmet, Armor, Gloves, Boots,
    Ring, Bracelet, Amulet, Artefact
}

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Objects/Items/Equipment/Equipment Item")]
public class EquipmentItem : Item
{
#if UNITY_EDITOR
    public override void OnValidate()
    {
        base.OnValidate();
        if (Modifiers.Length == 0)
        {
            Modifiers = new Modifier[(int)_Rarity + 1];
        }
    }

    [ButtonMethod]
    private void SetRandomBuffs()
    {
        for (int i = 0; i < Modifiers.Length; i++)
        {
            Modifiers[i] = new Modifier((StatType)UnityEngine.Random.RandomRange(0, Enum.GetNames(typeof(StatType)).Length), UnityEngine.Random.RandomRange(5, (int)_Rarity + 10) * 2, true);

            Modifiers[i].ModifierAmountType = ModifierAmountType.Procent;

            Modifiers[i].Element = (Element)UnityEngine.Random.RandomRange(0, Enum.GetNames(typeof(Element)).Length);
        }
    }
#endif

    public EquipmentItem()
    {
        Type = ItemType.Equipment;
        IsStackable = false;
    }

    public EquipmentType EquipmentType;
    public Modifier[] Modifiers;

    [HideInInspector]
    public bool IsEquiped = false;

    public override void UseItem(CharacterScript character)
    {
        var inventory = GameManager.Instance.inventory;

        if (IsEquiped & inventory.Inventory.Count < inventory.Inventory.Capacity)
        {
            character.equipment.UnequipItem(this);
        }
        else
        {
            character.equipment.EquipItem(this);
        }
    }
}