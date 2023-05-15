using UI;
using UnityEngine;

namespace OnLoaded
{
    internal class OnHubLoaded : MonoBehaviour
    {
        [SerializeField]
        private DialogueSystem.Dialogue firstDialogue;

        private void Start()
        {
            if (SaveLoadSystem.SaveLoadSystem.SavedData == null)
            {
                StartCoroutine(MiscUtilities.Instance.ActionWithDelay(0.1f, () =>
                {
                    DialogueSystem.DialogueManager.Instance.gameObject.SetActive(true);
                    DialogueSystem.DialogueManager.Instance.StartDialogue(firstDialogue);
                }));
            }

            RunManager.ResetSettings();

#if !UNITY_EDITOR
            SaveLoadSystem.SaveLoadSystem.Save(); 
#endif
        }
    }
}