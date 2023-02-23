using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript Instance;
    public int Capacity = 20;
    public List<Item> Inventory;
    public int Gold;
    public int Tokens;
    public int Score;

    public void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        Inventory.Capacity = Capacity;
    }

    public void AddItemToInventory(Item Item)
    {
        Item NewItem = Instantiate(Item);

        if (Inventory.Count < Inventory.Capacity)
        {
            if (Inventory.Contains(NewItem) & NewItem.IsStackable)
            {
                Inventory[Inventory.IndexOf(NewItem)].Amount += 1;
            }
            else
            {
                Inventory.Add(NewItem);
            }
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