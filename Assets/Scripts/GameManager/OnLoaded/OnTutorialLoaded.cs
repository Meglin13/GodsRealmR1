using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OnTutorialLoaded : MonoBehaviour
{
    [SerializeField]
    private DialogueSystem.Dialogue MarfaDialogue;

    private void Awake()
    {
        RunManager.SetDifficulty(1);
    }

    private void Start()
    {
        StartCoroutine(MiscUtilities.Instance.ActionWithDelay(0.1f, () =>
        {
            DialogueSystem.DialogueManager.Instance.gameObject.SetActive(true);
            DialogueSystem.DialogueManager.Instance.StartDialogue(MarfaDialogue);
        }));
    }

    private void OnDestroy()
    {
        RunManager.ResetSettings();
    }
}