using MyBox;
using UnityEngine;

public enum EquipmentType
{
    Helmet, Gloves, Armor,
    Ring, Bracelet, Amulet
}

[CreateAssetMenu(fileName = "EquipmentItem", menuName = "Objects/Equipment Item")]
public class EquipmentItem : Item
{
    public EquipmentItem()
    {
        Type = ItemType.Equipment;
        IsStackable = false;
    }

    public EquipmentType EquipmentType;
    public Modifier[] Modifiers;

    [ButtonMethod]
    public void SetItemModifiers()
    {
        Modifiers = new Modifier[(int)Rarity];
    }
}