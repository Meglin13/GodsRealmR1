using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    private float RotateX;
    private float RotateY;
    private float RotateZ;

    private Color color;
    private float hue = 0;
    public float hueChangeSpeed = 0.001f;

    public List<Material> Skyboxes = new List<Material>();

    void Start()
    {
        hue = Random.Range(0, 255);

        RotateX = Random.Range(-1f, 1f);
        RotateY = Random.Range(-1f, 1f);
        RotateZ = Random.Range(-1f, 1f);

        if (Skyboxes.Count > 0)
        {
            RenderSettings.skybox = Skyboxes[Random.RandomRange(0, Skyboxes.Count - 1)];
        }
    }

    void Update()
    {
        gameObject.transform.Rotate(RotateX * Time.deltaTime, RotateY * Time.deltaTime, RotateZ * Time.deltaTime);

        ColorManager.ChangeHue(ref hue, hueChangeSpeed, Time.deltaTime);

        color = Color.HSVToRGB(hue, 0.4f, 1);

        RenderSettings.skybox.SetColor("_Tint", color);
    }
}
