using UnityEngine;

public class Marfa : CharacterScript, ICharacter
{
    public GameObject NormalAttackBullet;
    public GameObject DistractSkillTotem;

    public override void Initialize()
    {
        Character = this;
        base.Initialize();
    }

    public void Attack()
    {
        Skill attackSkill = new Skill(1f, EntityStats.NormalAttackMult);

        MiscUtilities.Instance.ThrowThrowable(NormalAttackBullet, this, attackSkill);
    }

    public void HoldAttack()
    {

    }

    public void DistractionAbility()
    {
        LookAtEnemy(5f);
        Skill skill = EntityStats.SkillSet[SkillType.Distract];
        
        var totem = MiscUtilities.SpawnObjectInFrontOfObject(gameObject, DistractSkillTotem, 5);
        totem.GetComponent<AreaOfEffectScript>().Init(EntityStats, skill);
    }

    public void SpecialAbility()
    {
        GameManager.Instance.partyManager.GiveSupportToAll(EntityStats.ModifiableStats[StatType.Health].GetProcent() * EntityStats.SkillSet[SkillType.Special].DamageMultiplier.GetFinalValue(), StatType.Health);
    }

    public void UltimateAbility()
    {

    }
}