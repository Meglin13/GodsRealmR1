using UnityEngine;

public class ChestScript : MonoBehaviour, IInteractable
{
    public void Interaction()
    {
        GetComponent<Animator>().SetTrigger("OpenChest");

        InventoryScript inventory = GameManager.Instance.inventory;
    }
}