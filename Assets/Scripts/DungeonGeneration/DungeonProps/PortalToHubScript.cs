using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interactables
{
    [RequireComponent(typeof(BoxCollider))]
    public class PortalToHubScript : MonoBehaviour, IInteractable
    {
        public bool CanInteract() => true;
        public void Interaction()
        {
            if (SceneManager.GetActiveScene().name == "TutorialScene" | RunManager.CurrentFloor == RunManager.Params.FloorsAmount)
            {
                UIManager.Instance.ChangeScene("HubScene", null);
                RunManager.ResetSettings();
            }
            else
            {
                RunManager.NewFloor();
                DungeonGeneration.DungeonGeneratorScript.Instance.GenerateDungeon();
            }
        }
    }
}