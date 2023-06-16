using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public static InventoryScript Instance;
    public int Capacity = 30;

    public event Action OnInventoryChanged = delegate { };

    [SerializeField]
    private List<Item> inventory = new List<Item>();
    public List<Item> Inventory
    {
        get
        {
            inventory = inventory.Where(item => item != null || (item is EquipmentItem equip && !equip.IsEquiped)).ToList();
            inventory.Capacity = Capacity;

            return inventory;
        }
    }

    public int Gold;
    public int Tokens;
    public int Score;

    public void OnEnable() => Initialize();
    public void OnDisable()
    {
        OnInventoryChanged = null;
    }

    public void Initialize()
    {
        Instance = this;
        inventory = new List<Item>();
        inventory.Capacity = Capacity;
    }

    public void AddCapacity(int cap) => Inventory.Capacity += cap;

    /// <summary>
    /// Добавление предметов в инвентарь. SO автоматически спавнятся, так что нет смысла несколько раз из разворачивать
    /// </summary>
    /// <param name="Item"></param>
    /// <returns>Возвращает произошло ли добавление в инвентарь</returns>
    public bool AddItemToInventory(Item Item, bool IsNew)
    {
        if (Inventory.Count < Inventory.Capacity)
        {
            if (Inventory.Where(x => x.ID == Item.ID).FirstOrDefault() & Item.IsStackable)
            {
                Inventory[Inventory.IndexOf(Inventory.Where(x => x.Name == Item.Name).FirstOrDefault())].Amount += 1;
            }
            else
            {
                Item NewItem = Item.InstanceID == string.Empty ? Item.GetCopy() : Item;
                //Item NewItem = Item.GetCopy();

                if (Item is EquipmentItem equip)
                {
                    equip.IsEquiped = false;
                }

                Inventory.Add(NewItem);
            }

            OnInventoryChanged();

            return true;
        }

        return false;
    }

    public void DeleteItem(Item item, bool IsDestroy = false)
    {
        Item ItemForDelete = Inventory.Where(x => x.InstanceID == item.InstanceID).First();

        if (ItemForDelete.Amount > 1 & ItemForDelete.IsStackable)
        {
            ItemForDelete.Amount--;
        }
        else
        {
            Inventory.Remove(ItemForDelete);
            if (IsDestroy)
                Destroy(ItemForDelete);
        }

        OnInventoryChanged();
    }
}