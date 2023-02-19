using System;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Dummy : EnemyScript, IEnemy
{
    public override void Start()
    {
        base.Start();
        Enemy = this;
    }

    public override void Update()
    {
        base.Update();
    }

    public void Attack()
    {

    }

    public void SpecialPower()
    {
     
    }

    public override void Death()
    {
        CurrentHealth = EntityStats.ModifiableStats[StatType.Health].GetFinalValue();
    }
}