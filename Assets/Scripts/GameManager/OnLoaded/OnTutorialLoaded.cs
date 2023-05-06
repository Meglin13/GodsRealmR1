using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class OnTutorialLoaded : MonoBehaviour
{
    private void Awake()
    {
        RunManager.SetDifficulty(1);
    }
}