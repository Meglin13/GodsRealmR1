using UnityEngine;

public class CameraCenterBehaviour : MonoBehaviour
{
    public static CameraCenterBehaviour Instance;
    private Transform target;
    public GameObject InteractionButton;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (target)
            transform.SetPositionAndRotation(target.position, transform.parent.rotation);
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
        this.transform.SetParent(transform);
    }
}