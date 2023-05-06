using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpherizeNormalCenterObject : MonoBehaviour
{
    [SerializeField]
    private Renderer m_renderer;

    [SerializeField]
    private int m_targetMaterialSubMeshIndex;

    private int m_spherizeNormalPropId;

    private void Awake()
    {
        m_spherizeNormalPropId = Shader.PropertyToID("_SpherizeNormalOrigin");
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

#if UNITY_EDITOR

    private void OnValidate()
    {
    }

#endif

    // Update is called once per frame
    private void Update()
    {
        if (m_renderer == null)
            return;
        if (Application.isEditor)
        {
            m_renderer.sharedMaterials[m_targetMaterialSubMeshIndex].SetVector(m_spherizeNormalPropId, transform.position);
        }
        else
        {
            m_renderer.materials[m_targetMaterialSubMeshIndex].SetVector(m_spherizeNormalPropId, transform.position);
        }
    }
}