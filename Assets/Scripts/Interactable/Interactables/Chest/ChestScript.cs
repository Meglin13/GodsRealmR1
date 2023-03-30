using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public ChestStats chestStats;
    private InventoryScript Inventory;
    private bool IsOpened = false;
    public bool CanOpen = false;

    public void Awake()
    {
        Inventory = GameManager.Instance.inventory;
    }

    public bool CanInteract()
    {
        return CanOpen;
    }

    public void Interaction()
    {
        if (!IsOpened)
        {
            GetComponent<Animator>().SetTrigger("OpenChest");

            RandomChances chances = new RandomChances(GameManager.Instance.ItemsList);

            List<Item> items = chances.GetItems(Random.RandomRange(chestStats.MinItems, chestStats.MaxItems));

            foreach (Item item in items)
            {
                if (!Inventory.AddItemToInventory(item))
                    break;
            }

            Inventory.Gold += Random.RandomRange(chestStats.MinGold, chestStats.MaxGold);

            IsOpened = true;

            Destroy(this, 1f);
        }
    }
}