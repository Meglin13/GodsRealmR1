using UnityEngine;

public class Marfa : CharacterScript, ICharacter
{
    public GameObject NormalAttackBullet;

    public override void Awake()
    {
        Character = this;
        base.Awake();
    }

    public void Attack()
    {
        LookAtEnemy(EntityStats.AttackRange);

        Skill attackSkill = new Skill()
        {
            Radius = 1f,
            BaseDamageMultiplier = EntityStats.NormalAttackMult,
        };
        attackSkill.DamageMultiplier = new Stat(attackSkill.BaseDamageMultiplier);

        MiscUtilities.Instance.ThrowThrowable(NormalAttackBullet, this, attackSkill);
    }

    public void HoldAttack()
    {

    }

    public void DistractionAbility()
    {
        Debug.Log("Distraction Attack");
    }

    public void SpecialAbility()
    {
        GameManager.GetInstance().partyManager.GiveSupportToAll(EntityStats.ModifiableStats[StatType.Health].GetProcent() * EntityStats.SkillSet[SkillsType.Special].DamageMultiplier.GetFinalValue(), StatType.Health);
    }

    public void UltimateAbility()
    {

    }
}