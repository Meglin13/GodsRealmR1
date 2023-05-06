using ObjectPooling;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Marfa : CharacterScript, ICharacter
{
    public ThrowableScript NormalAttackBullet;

    public override void Initialize()
    {
        Character = this;
        base.Initialize();
    }

    public void Attack()
    {
        Skill attackSkill = new Skill(1f, EntityStats.NormalAttackMult);

        Transform transform = gameObject.transform;
        Vector3 gunpoint = transform.position + transform.forward + transform.up;

        SpawnablePool.Instance.CreateObject(NormalAttackBullet, gunpoint, this, attackSkill);
    }

    public void DistractionAbility()
    {
        var totem = MiscUtilities.SpawnObjectInFrontOfObject(this, Distract.SpawningObject, 5);
        totem.Spawn(this, Distract);
    }

    public void SpecialAbility()
    {
        GameManager.Instance.partyManager.GiveSupportToAll(EntityStats.ModifiableStats[StatType.Health].GetProcent() * Special.DamageMultiplier.GetFinalValue(), StatType.Health);
    }

    public void UltimateAbility()
    {

    }
}