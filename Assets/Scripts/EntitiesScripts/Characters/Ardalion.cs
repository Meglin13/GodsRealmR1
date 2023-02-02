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
        LookAtEnemy();
        GameObject bomb = Instantiate(DistractionThrowable);
        Skill skillInfo = EntityStats.SkillSet[SkillsType.Distract];
        bomb.GetComponent<ExplosivesScript>().Init(EntityStats, skillInfo.DamageMultiplier.GetFinalValue(), skillInfo.Radius, skillInfo.DurationInSec);
        bomb.GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * 5f, ForceMode.Impulse);
    }

    Modifier WeaponLengthPlus = new Modifier(StatType.WeaponLength, 2f);

    public void SpecialAbility()
    {
        AddModifier(WeaponLengthPlus);
    }

    public void UltimateAbility()
    {
        Debug.Log("Ultimate Ability");
    }
}