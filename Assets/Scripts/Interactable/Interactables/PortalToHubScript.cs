using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;

namespace Interactables
{
    [RequireComponent(typeof(BoxCollider))]
    internal class PortalToHubScript : MonoBehaviour, IInteractable
    {
        public bool CanInteract() => true;
        public void Interaction() => UIManager.Instance.ChangeScene("HubScene", null);
    }
}