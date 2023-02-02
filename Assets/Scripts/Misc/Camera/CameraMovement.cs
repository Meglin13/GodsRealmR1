using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;
    [SerializeField] private float smoothTime;
    private Vector3 _currentVelocity = Vector3.zero;

    void Start()
    {
        target = GameObject.FindWithTag("CameraCenter").transform;
        offset = gameObject.transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}