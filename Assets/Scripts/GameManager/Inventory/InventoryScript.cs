using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: Сделать инвентарь

public class InventoryScript : MonoBehaviour
{
    public int Capacity = 20;
    private List<Item> Inventory;
    public int Gold;
    public int Tokens;
    public int Score;

    public void Initialize()
    {
        Inventory = new List<Item>(Capacity);
    }

    public void AddItemToInventory(Item Item)
    {
        Item NewItem = Item;

        if (Inventory.Count <= Inventory.Capacity)
        {
            if (Inventory.Contains(NewItem))
                Inventory[Inventory.IndexOf(NewItem)].Amount += 1;
            else
                Inventory.Add(NewItem);
        }
    }

    public void DeleteItem(Item ItemForDelete)
    {
        Inventory.Remove(ItemForDelete);
    }

    //Сортировка по:
    // 1 - Имени
    // 2 - Кол-ву
    // 3 - Типу

    public List<Item> SortByInventory(int SortingMode)
    {
        List<Item> SortedInventory = new List<Item>();

        switch (SortingMode)
        {
            case 1:
                SortedInventory = Inventory.OrderBy(x => x.Name).ToList();
                break;

            case 2:
                SortedInventory = Inventory.OrderBy(x => x.Amount).ToList();
                break;

            case 3:
                SortedInventory = Inventory.OrderBy(x => (int)x.Type).ToList();
                break;
        }

        return SortedInventory;
    }

    //TODO: Экипировка предметов
    public void EquipItem(EquipmentItem item, CharacterScript character)
    {
        foreach (var ad in item.Modifiers)
        {
            character.EntityStats.ModifiableStats[ad.StatType].AddModifier(ad);
        }
    }

    public void UnequipItem(EquipmentItem item, CharacterScript character)
    {
        foreach (var ad in item.Modifiers)
        {
            character.EntityStats.ModifiableStats[ad.StatType].RemoveModifier(ad);
        }
    }
}