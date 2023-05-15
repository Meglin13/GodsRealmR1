using MyBox;
using UnityEngine;

public class HueChangeScript : MonoBehaviour
{
    Material material;
    float hue = 0;
    [SerializeField]
    float hueChangeSpeed = 0.001f;
    [SerializeField, PositiveValueOnly]
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().sharedMaterials[index];
        Color.RGBToHSV(material.color, out hue, out float s, out float v);
    }

    // Update is called once per frame
    void Update()
    {
        material.color = ColorManager.ChangeHue(ref hue, hueChangeSpeed, Time.deltaTime);
    }
}