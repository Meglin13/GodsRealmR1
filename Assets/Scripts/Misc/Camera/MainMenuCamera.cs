using System.Collections;
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

        RotateX = Random.Range(-0.02f, 0.02f);
        RotateY = Random.Range(-0.02f, 0.02f);
        RotateZ = Random.Range(-0.02f, 0.02f);

        RenderSettings.skybox = Skyboxes[Random.RandomRange(0, Skyboxes.Count)];
    }

    void Update()
    {
        gameObject.transform.Rotate(RotateX, RotateY, RotateZ);

        ChangeHue();
        color = Color.HSVToRGB(hue, 0.4f, 1);

        RenderSettings.skybox.SetColor("_Tint", color);
    }

    void ChangeHue()
    {
        hue += hueChangeSpeed * Time.deltaTime;

        if (hue > 1f | hue < 0f)
            hue = 0f;
    }
}
