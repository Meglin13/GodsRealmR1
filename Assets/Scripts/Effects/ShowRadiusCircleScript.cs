using MyBox;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ShowRadiusCircleScript : MonoBehaviour
{
    [SerializeField]
    private Color circleColor;

    [Range(3, 256)]
    public int numSegments = 128;

    [ButtonMethod]
    public void Draw(float radius)
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Additive"));
        lineRenderer.SetColors(circleColor, circleColor);
        lineRenderer.SetWidth(0.2f, 0.2f);
        lineRenderer.SetVertexCount(numSegments + 1);
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(2.0 * Mathf.PI) / numSegments;
        float theta = 0f;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, 0, z);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}