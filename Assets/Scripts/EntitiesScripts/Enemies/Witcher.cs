using ObjectPooling;
using UnityEngine;

public class Witcher : EnemyScript, IEnemy
{
    public override void Start()
    {
        base.Start();
        Enemy = this;
    }

    private void Update()
    {
        FollowAndAttack();
    }

    public void Attack()
    {
        Skill attackSkill = EntityStats.NormalAttackSkill;

        Transform transform = gameObject.transform;
        Vector3 gunpoint = transform.position + transform.forward + transform.up;

        var attack = SpawnablePool.Instance.CreateObject(attackSkill.SpawningObject, gunpoint);
        attack.Spawn(this, attackSkill);
    }

    public void SpecialPower()
    {
        Skill attackSkill = EntityStats.NormalAttackSkill;

        Transform transform = gameObject.transform;
        Vector3 gunpoint = transform.position + transform.forward + transform.up;

        var attack = SpawnablePool.Instance.CreateObject(attackSkill.SpawningObject, gunpoint);
        attack.Spawn(this, attackSkill);
    }
}

