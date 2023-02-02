using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public GameObject gameObject { get; }
    public EntityStats DealerStats { get; set; }
    public float Mult { get; set; }
    public float Length { get; set; }
    public List<Collider> hasHitted { get; set; }
    public void Init(EntityStats entityStats, float Mult, float Radius, float StunTime);
}