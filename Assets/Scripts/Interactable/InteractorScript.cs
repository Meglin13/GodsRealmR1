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

        InteractionButton = FindAnyObjectByType<CameraCenterBehaviour>().InteractionButton;
        InteractionButton.SetActive(false);

        interactionLayer = 1 << AIUtilities.InteractLayer;
    }

    private void Update()
    {
        interactColliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);

        IInteractable interactable = null;

        bool exp = interactColliders.Length > 0
            && interactColliders[0].TryGetComponent(out interactable)
            && interactable.CanInteract();

        InteractionButton.SetActive(exp);

        if (exp && interact.triggered)
        {
            interactable.Interaction();
        }
    }
}