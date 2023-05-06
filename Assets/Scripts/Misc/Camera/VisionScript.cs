using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class VisionScript : MonoBehaviour
{
    Camera camera;
    [SerializeField]
    GameObject Overlay;

    public GameObject[] HiddenSecrets;

    InputAction vision;
    UniversalAdditionalCameraData additionalCameraData;

    bool IsVisionActive;

    void Awake()
    {
        camera = Camera.main;
        additionalCameraData = camera.transform.GetComponent<UniversalAdditionalCameraData>();

        vision = GetComponent<PlayerInput>().actions["Vision"];

        foreach (var item in HiddenSecrets)
        {
            if (item != null)
            {
                item.SetActive(false);
            }
        }
    }

    private void Update()
    {
        Vision(vision.activeControl != null);
    }

    void Vision(bool IsAction)
    {
        if (UIManager.Instance.CurrentMenu == null & IsVisionActive != IsAction)
        {
            IsVisionActive = IsAction;

            Overlay.gameObject.SetActive(IsAction);
            Time.timeScale = IsAction ? 0.6f : 1f;
            int renderIndex = IsAction ? 1 : 0;
            additionalCameraData.SetRenderer(renderIndex);

            foreach (var item in HiddenSecrets)
            {
                if (item != null)
                {
                    item.SetActive(IsAction);
                }
            }
        }
    }
}