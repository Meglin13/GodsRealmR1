using MyBox;
using UnityEngine;

public enum EquipmentType
{
    Helmet, Armor, Gloves, Boots,
    Ring, Bracelet, Amulet, Artefact
}

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Objects/Items/Equipment/Equipment Item")]
public class EquipmentItem : Item
{
    public EquipmentItem()
    {
        Type = ItemType.Equipment;
        IsStackable = false;
    }

    public EquipmentType EquipmentType;
    public Modifier[] Modifiers;

    [HideInInspector]
    public bool IsEquiped = false;

    [ButtonMethod]
    public void SetItemModifiers()
    {
        Modifiers = new Modifier[(int)_Rarity + 1];
    }

    public override void UseItem(CharacterScript character)
    {
        var inventory = GameManager.Instance.inventory;

        if (IsEquiped)
        {
            if (inventory.Inventory.Count < inventory.Inventory.Capacity)
            {
                character.equipment.UnequipItem(this);
            }
        }
        else
        {
            character.equipment.EquipItem(this);
        }
    }
}