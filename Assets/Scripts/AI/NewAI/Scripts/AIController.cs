using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public IDamageable entity;



    private void Awake()
    {
        entity = GetComponent<IDamageable>();
    }

}