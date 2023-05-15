using UnityEngine;
using UnityEngine.InputSystem;

public class InteractorScript : MonoBehaviour
{
    [SerializeField]
    float interactionRadius = 2f;
    int interactionLayer;
    Collider[] interactColliders = new Collider[3];

    InputAction interact;
    GameObject InteractionButton;

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        interact = playerInput.actions["Interact"];

        InteractionButton = GameObject.FindAnyObjectByType<CameraCenterBehaviour>().InteractionButton;
        InteractionButton.SetActive(false);

        interactionLayer = 1 << AIUtilities.InteractLayer;
    }

    private void Update()
    {
        interactColliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);

        InteractionButton.SetActive(interactColliders.Length > 0);

        if (interactColliders.Length > 0 & interact.triggered 
            && interactColliders[0].TryGetComponent(out IInteractable interactable) 
            && interactable.CanInteract())
            interactable.Interaction();
    }
}