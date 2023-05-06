using UnityEngine;

public class Skeleton : EnemyScript, IEnemy
{
    public override void Start()
    {
        base.Start();
        Enemy = this;
    }

    public override void Update()
    {
        base.Update();
        FollowAndAttack();
    }

    public void Attack()
    {

    }

    public void SpecialPower()
    {
        Debug.Log(gameObject.name + " has used Special Power!");
    }
}