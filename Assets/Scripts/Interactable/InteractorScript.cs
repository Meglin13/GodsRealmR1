using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractorScript : MonoBehaviour
{
    [SerializeField]
    float interactionRadius = 2f;
    int interactionLayer;
    Collider[] interactColliders = new Collider[3];

    InputAction interact;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];

        interactionLayer = 1 << AIUtilities.InteractLayer;
    }

    private void Update()
    {
        interactColliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);
        if (interactColliders.Length > 0 & interact.triggered)
        {
            if (interactColliders[0].TryGetComponent(out IInteractable interacable))
            {
                interacable.Interaction();
            }
        }
    }
}
