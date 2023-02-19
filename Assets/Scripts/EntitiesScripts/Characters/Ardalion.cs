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

    public void DistractionAbility()
    {
        GameObject bomb = Instantiate(DistractionThrowable, transform.position, transform.rotation);
        Skill skillInfo = EntityStats.SkillSet[SkillsType.Distract];

        MiscUtilities.ThrowThrowable(bomb, this, skillInfo);
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