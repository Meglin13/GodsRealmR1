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
        {
            gameObject.transform.position = target.position;

            gameObject.transform.rotation = gameObject.transform.parent.rotation;
        }
    }

    public void SetTarget(Transform transform)
    {
        target = transform;
        //gameObject.transform.position = target.position;
        gameObject.transform.SetParent(transform);
    }
}