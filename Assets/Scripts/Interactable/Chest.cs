using UnityEngine;

public class Chest : IInteractable
{
    public ChestStats chestStats;
    public InventoryScript Inventory;

    public void Interaction()
    {
        Inventory = GameObject.FindObjectOfType<InventoryScript>();


    }
}
