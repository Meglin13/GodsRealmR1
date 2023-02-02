using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveScript : MonoBehaviour
{
    public void SetActive()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
