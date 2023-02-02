using UnityEngine;

public class LookAtCameraScript : MonoBehaviour
{
    private Camera camera;
    void Start() => camera = Camera.main;
    void Update() => transform.rotation = camera.transform.rotation;
}