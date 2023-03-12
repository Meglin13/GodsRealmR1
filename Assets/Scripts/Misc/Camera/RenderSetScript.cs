using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSetScript : MonoBehaviour
{
    public Shader shader;

    void Start()
    {
        Camera.main.SetReplacementShader(shader, string.Empty);
    }
}
