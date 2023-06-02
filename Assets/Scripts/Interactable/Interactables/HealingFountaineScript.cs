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

                var currentHealth = 0f;
                PartyManager.Instance.PartyMembers.ForEach(x => currentHealth += x.EntityStats.Health.GetFinalValue());
                currentHealth /= PartyManager.Instance.PartyMembers.Count;

                PartyManager.Instance.GiveSupportToAll(currentHealth, StatType.Health);

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
