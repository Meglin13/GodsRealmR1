using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript Instance;
    public int Capacity = 30;
    public List<Item> Inventory;
    public int Gold;
    public int Tokens;
    public int Score;

    public void Awake()
    {
        Instance = this;
        Initialize();
    }

    public void Initialize()
    {
        Inventory.Capacity = Capacity;
    }

    public void AddCapacity(int cap)
    {
        Inventory.Capacity += cap;
    }

    /// <summary>
    /// Добавление предметов в инвентарь. SO автоматически спавнятся, так что нет смысла несколько раз из разворачивать
    /// </summary>
    /// <param name="Item"></param>
    /// <returns>Возвращает произошло ли добавление в инвентарь</returns>
    public bool AddItemToInventory(Item Item)
    {
        if (Inventory.Count < Inventory.Capacity)
        {
            Item NewItem = Instantiate(Item);

            if (Inventory.Contains(NewItem) & NewItem.IsStackable)
            {
                Inventory[Inventory.IndexOf(NewItem)].Amount += 1;
            }
            else
            {
                Inventory.Add(NewItem);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DeleteItem(string ItemID)
    {
        Item ItemForDelete = Inventory.Where(x => x.ID == ItemID).First();

        if (ItemForDelete.Amount > 1 & ItemForDelete.IsStackable)
        {
            ItemForDelete.Amount--;
        }
        else
        {
            Inventory.Remove(ItemForDelete);
            Destroy(ItemForDelete);
        }
    }

    public enum SortingMode { NoSort, Name, Quantity, Type }

    public List<Item> SortByInventory(SortingMode sortingMode, bool isDesc)
    {
        List<Item> SortedInventory = new List<Item>();

        switch (sortingMode)
        {
            case SortingMode.Name:
                SortedInventory = Inventory.OrderBy(x => x.Name).ToList();
                break;

            case SortingMode.Quantity:
                SortedInventory = Inventory.OrderBy(x => x.Amount).ToList();
                break;

            case SortingMode.Type:
                SortedInventory = Inventory.OrderBy(x => (int)x.Type).ToList();
                break;
        }

        if (isDesc)
            SortedInventory.Reverse();

        return SortedInventory;
    }
}