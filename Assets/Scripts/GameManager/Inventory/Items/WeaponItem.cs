using MyBox;
using UnityEngine;

public enum WeaponType
{
    OneHandSword, TwoHandSword, Bow, Crossbow, Pistol, Book, Stave, Axe
}

//TODO: Решить судьбу оружки
[CreateAssetMenu(fileName = "WeaponItem", menuName = "Objects/WeaponType Item")]
public class WeaponItem : EquipmentItem
{
    public WeaponType WeaponType;
}