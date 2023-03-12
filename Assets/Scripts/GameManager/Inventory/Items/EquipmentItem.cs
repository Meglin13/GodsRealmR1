using MyBox;
using UnityEngine;

public enum EquipmentType
{
    Helmet, Armor, Gloves, Boots,
    Ring, Bracelet, Amulet, Artefact
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
    [HideInInspector]
    public bool IsEquiped = false;

    [ButtonMethod]
    public void SetItemModifiers()
    {
        Modifiers = new Modifier[(int)_Rarity + 1];

        //for (int i = 0; i < Modifiers.Length; i++)
        //{
        //    Modifiers[i].IsPermanent = true;
        //}
    }
}