using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ITarget
{
    public GameObject gameObject { get; }
    public byte Priority { get; }
}