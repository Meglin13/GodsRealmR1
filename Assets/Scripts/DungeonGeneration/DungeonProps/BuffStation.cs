using UI;
using UnityEngine;

namespace DungeonGeneration.DungeonProps
{
    public class BuffStation : MonoBehaviour, IInteractable
    {
        private bool IsInteracted = false;

        public bool CanInteract()
        {
            return !IsInteracted;
        }

        public void Interaction()
        {
            IsInteracted = true;
            UIManager.Instance.OpenMenu(UIManager.Instance.BuffScreen);
        }
    }
}