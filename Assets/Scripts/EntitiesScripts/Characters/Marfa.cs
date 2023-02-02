using UnityEngine;

public class Marfa : CharacterScript, ICharacter
{
    public override void Awake()
    {
        Character = this;
        base.Awake();
    }

    public void Attack()
    {
        MiscUtilities.ThrowBullet(NormalAttackBullet, EntityStats.NormalAttackMult, transform.position + transform.forward, this);
    }

    public void DistractionAbility()
    {
        Debug.Log("Distraction Attack");
    }

    public void SpecialAbility()
    {
        GameManager.Instance.partyManager.GiveHealToAll(EntityStats.ModifiableStats[StatType.Health].GetProcent() * EntityStats.SkillSet[SkillsType.Special].DamageMultiplier.GetFinalValue());
    }

    public void UltimateAbility()
    {

    }
}