using UnityEngine;

[SelectionBase]
public class Dummy : EnemyScript, IEnemy
{
    public override void Start()
    {
        base.Start();
        Enemy = this;
    }

    public void Attack()
    {

    }

    public void SpecialPower()
    {

    }

    public override void Death()
    {
        CurrentHealth = 1000;
    }
}