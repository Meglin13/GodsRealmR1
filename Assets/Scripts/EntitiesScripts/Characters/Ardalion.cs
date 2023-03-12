using UnityEngine;

public class Ardalion : CharacterScript, ICharacter
{
    public GameObject DistractionThrowable;

    public override void Awake()
    {
        Character = this;
        base.Awake();
    }

    public void Attack()
    {

    }

    public void HoldAttack()
    {

    }

    public void DistractionAbility()
    {
        Skill skillInfo = EntityStats.SkillSet[SkillsType.Distract];

        MiscUtilities.Instance.ThrowThrowable(DistractionThrowable, this, skillInfo);
    }

    Modifier WeaponLengthPlus = new Modifier(StatType.WeaponLength, 2f);

    public void SpecialAbility()
    {
        
    }

    public void UltimateAbility()
    {
        Debug.Log("Ultimate Ability");
    }
}