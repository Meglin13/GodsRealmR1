using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
    private Camera camera;
    void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = camera.transform.rotation;
    }
}