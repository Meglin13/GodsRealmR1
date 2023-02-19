using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform target;
    private Vector3 offset;

    void Awake()
    {
        target = GameObject.FindWithTag("CameraCenter").transform;
        offset = gameObject.transform.position - target.position;
    }

    private void LataUpdate()
    {
        transform.position = target.position + offset;
    }
}