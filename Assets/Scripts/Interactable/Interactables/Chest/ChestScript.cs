using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public ChestStats chestStats;
    private InventoryScript Inventory;
    private bool IsOpened = false;

    public void Awake()
    {
        Inventory = GameManager.Instance.inventory;
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interaction()
    {
        IsOpened = true;

        GetComponentInChildren<Animator>().SetTrigger("OpenChest");

        RandomChances equipment = new RandomChances(GameManager.Instance.EquipmentList.Cast<Item>().ToList());
        RandomChances potions = new RandomChances(GameManager.Instance.PotionList.Cast<Item>().ToList());

        List<Item> equipmentList = equipment.GetItems(Random.RandomRange(chestStats.Items.Min, chestStats.Items.Max));
        List<Item> potionsList = potions.GetItems(Random.RandomRange(chestStats.Potions.Min, chestStats.Potions.Max));

        var items = equipmentList.Concat(potionsList).ToList();

        foreach (Item item in items)
        {
            if (!Inventory.AddItemToInventory(item, true))
                break;
        }

        Inventory.Gold += Random.RandomRange(chestStats.Gold.Min, chestStats.Gold.Max);

        StartCoroutine(MiscUtilities.Instance.ActionWithDelay(2f, () => gameObject.SetActive(false)));

    }
}