using MyBox;
using UnityEngine;

public enum WeaponType
{
    OneHandSword, TwoHandSword, Bow, Crossbow, Pistol, Book, Stave, Axe
}

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Objects/WeaponType Item")]
public class WeaponItem : Item
{
    public WeaponItem()
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