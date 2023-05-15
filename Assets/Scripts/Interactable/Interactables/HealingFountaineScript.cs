using TMPro;
using UI;
using UnityEngine;

namespace Interactables
{
    internal class HealingFountaineScript : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private int Price = 30;
        private bool CanOpen = true;
        [SerializeField]
        private TextMeshProUGUI text;

        public bool CanInteract()
        {
            return CanOpen;
        }

        public void Interaction()
        {
            if (InventoryScript.Instance.Gold >= Price)
            {
                CanOpen = false;
                text.gameObject.SetActive(false);

                InventoryScript.Instance.Gold -= Price;

                //var health = 0f;
                //PartyManager.Instance.PartyMembers.ForEach(x => health += x.EntityStats.Health.GetFinalValue());
                //health = health / PartyManager.Instance.PartyMembers.Count;

                //PartyManager.Instance.GiveSupportToAll(health, StatType.Health);

                foreach (var item in PartyManager.Instance.PartyMembers)
                    item.GiveSupport(item.EntityStats.Health.GetFinalValue() * 0.3f, StatType.Health);
            }
            else
                text.color = Color.red;
        }

        private void Awake()
        {
            Price = Random.Range(Price - 10, Price + 20);
            text.text = Price.ToString();
        }
    }
}
