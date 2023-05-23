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
        public void Interaction()
        {
            if (RunManager.CurrentFloor == RunManager.Params.FloorsAmount)
            {
                UIManager.Instance.ChangeScene("HubScene", null);
                RunManager.ResetSettings();
            }
            else
            {
                Debug.Log("New Floor");
                RunManager.NewFloor();
                DungeonGeneration.DungeonGeneratorScript.Instance.GenerateDungeon();
            }
        }
    }
}