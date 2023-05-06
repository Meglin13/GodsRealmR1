using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 3f)]
    private float LifeTime = 1f;

    private void OnEnable()
    {
        StartCoroutine(MiscUtilities.Instance.ActionWithDelay(LifeTime, () => gameObject.SetActive(false)));
    }
}