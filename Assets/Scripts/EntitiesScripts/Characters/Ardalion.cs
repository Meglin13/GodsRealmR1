using UnityEngine;

public class Ardalion : CharacterScript, ICharacter
{
    public GameObject DistractionThrowable;
    public GameObject UltimateSlash;

    private bool IsUltimateActive = false;

    public override void Initialize()
    {
        Character = this;
        base.Initialize();
    }

    public void Attack()
    {
        if (IsUltimateActive)
        {
            MiscUtilities.Instance.ThrowThrowable(UltimateSlash, this, Ultimate);
        }
    }

    public void HoldAttack()
    {

    }

    public void DistractionAbility()
    {
        Skill skill = EntityStats.skills[1];

        MiscUtilities.Instance.ThrowThrowable(DistractionThrowable, this, skill);
    }

    Modifier WeaponLengthPlus = new Modifier(StatType.WeaponLength, 3f);

    public void SpecialAbility()
    {
        StartCoroutine(AddModifier(WeaponLengthPlus));

        MeleeWeapon.Init(EntityStats, Special.DamageMultiplier.GetFinalValue(), false);
    }

    public void UltimateAbility()
    {
        IsUltimateActive = true;
        StartCoroutine(MiscUtilities.Instance.ActionWithDelay(Ultimate.DurationInSecs, () => IsUltimateActive = false));
    }
}