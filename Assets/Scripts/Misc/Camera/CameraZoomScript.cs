using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomScript : MonoBehaviour
{
    Camera _camera;
    float CurrentSize;
    public float scrollSpeed  = 0.001f;
    public float MaxSize = 4;
    public float MinSize = 6;

    InputAction scroll;

    private void Start()
    {
        _camera = Camera.main;
        CurrentSize = _camera.orthographicSize;
        var playerInput = GetComponent<PlayerInput>();

        scroll = playerInput.actions["Scroll"];
    }

    private void Update()
    {
        CurrentSize -= scroll.ReadValue<float>() * scrollSpeed * Time.deltaTime;

        float NewSize = Mathf.Clamp(CurrentSize, MaxSize, MinSize);

        _camera.orthographicSize = NewSize;
    }

    //TODO: Проправить зум
}
