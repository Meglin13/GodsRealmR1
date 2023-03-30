using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine;

public class HubPortalScript : MonoBehaviour, IInteractable
{
    public GameObject RunSetupUI;

    private void Awake()
    {
        gameObject.layer = AIUtilities.InteractLayer;
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interaction()
    {
        UIManager.Instance.OpenMenu(RunSetupUI);
    }
}