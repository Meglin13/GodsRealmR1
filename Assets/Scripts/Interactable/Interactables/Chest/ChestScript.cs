using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public ChestStats chestStats;
    private InventoryScript Inventory;
    private bool IsOpened = false;

    private List<Item> Items;

    private static RandomChances equipment;
    private static RandomChances potions;

    public void Start()
    {
        Inventory = GameManager.Instance.inventory;

         equipment ??= new RandomChances(GameManager.Instance.EquipmentList.Cast<Item>().ToList());
         potions ??= new RandomChances(GameManager.Instance.PotionList.Cast<Item>().ToList());

        List<Item> equipmentList = equipment.GetItems(Random.RandomRange(chestStats.Items.Min, chestStats.Items.Max));
        List<Item> potionsList = potions.GetItems(Random.RandomRange(chestStats.Potions.Min, chestStats.Potions.Max));

        Items = equipmentList.Concat(potionsList).ToList();
    }

    public bool CanInteract()
    {
        return !IsOpened;
    }

    public void Interaction()
    {
        IsOpened = true;

        if (Items != null)
        {
            GetComponentInChildren<Animator>().SetTrigger("OpenChest");

            foreach (Item item in Items)
            {
                if (!Inventory.AddItemToInventory(item, true))
                    break;
            }

            Inventory.Gold += Random.RandomRange(chestStats.Gold.Min, chestStats.Gold.Max);

            StartCoroutine(MiscUtilities.Instance.ActionWithDelay(1.5f, () => gameObject.SetActive(false))); 
        }
        else
        {
            //TODO: Уведомление в менеджер уведомлений
            Debug.Log("Ничего нет!");
        }
    }
}