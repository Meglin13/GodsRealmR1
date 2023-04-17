using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomScript : MonoBehaviour
{
    Camera _camera;
    public Camera MinimapCamera;

    float CurrentSize;
    public float scrollSpeed  = 0.001f;
    public float MaxSize = 6;
    public float MinSize = 4;

    public float MaxSizeMinimap = 20;
    public float MinSizeMinimap = 10;

    InputAction scroll;
    InputAction ZoomInMiniMap;
    InputAction ZoomOutMiniMap;

    private void Start()
    {
        _camera = Camera.main;
        CurrentSize = _camera.orthographicSize;
        var playerInput = GetComponent<PlayerInput>();

        scroll = playerInput.actions["Scroll"];

        ZoomOutMiniMap = playerInput.actions["ZoomOutMinimap"];
        ZoomInMiniMap = playerInput.actions["ZoomInMinimap"];

        ZoomInMiniMap.performed += ctx => ZoomMiniMap(0.5f);
        ZoomOutMiniMap.started += ctx => ZoomMiniMap(-0.5f);
    }

    private void ZoomMiniMap(float zoom)
    {
        MinimapCamera.orthographicSize = Mathf.Clamp(MinimapCamera.orthographicSize + zoom * Time.deltaTime, MinSizeMinimap, MaxSizeMinimap);
    }

    private void Update()
    {
        CurrentSize -= scroll.ReadValue<float>() * scrollSpeed * Time.deltaTime;

        float NewSize = Mathf.Clamp(CurrentSize, MaxSize, MinSize);

        _camera.orthographicSize = NewSize;
    }

    //TODO: Проправить зум
}
