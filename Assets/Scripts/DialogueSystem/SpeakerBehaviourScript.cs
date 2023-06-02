using System;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace DialogueSystem
{
    public class SpeakerBehaviourScript : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private Dialogue dialogue;

        public UnityEvent OnDialogueEnd;

        public bool CanInteract()
        {
            return dialogue != null;
        }

        public void Interaction()
        {
            DialogueManager.Instance.StartDialogue(dialogue, OnDialogueEnd);
        }
    }
}
