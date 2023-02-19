using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{

    public ChestStats chestStats;
    public InventoryScript Inventory;

    public void Awake()
    {
        Inventory = GameManager.GetInstance().inventory;
    }

    public void Interaction()
    {
        GetComponent<Animator>().SetTrigger("OpenChest");

        //TODO: Получение предмтов из сундуков
    }
}