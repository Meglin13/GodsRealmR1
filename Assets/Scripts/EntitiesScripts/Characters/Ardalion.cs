using ObjectPooling;
using UnityEngine;

public class Ardalion : CharacterScript, ICharacter
{

    private bool IsUltimateActive = false;

    public override void Initialize()
    {
        Character = this;
        base.Initialize();
        IsUltimateActive = false;
    }

    public void Attack()
    {
        if (IsUltimateActive)
        {
            Transform transform = gameObject.transform;
            Vector3 gunpoint = transform.position + transform.forward + transform.up;

            SpawnablePool.Instance.CreateObject(Ultimate.SpawningObject, gunpoint, this, Ultimate);
        }
    }

    public void DistractionAbility()
    {
        Transform transform = gameObject.transform;
        Vector3 gunpoint = transform.position + transform.forward + transform.up;

        Debug.Log($"{this} {Distract}");
        SpawnablePool.Instance.CreateObject(Distract.SpawningObject, gunpoint, this, Distract);
    }

    public void SpecialAbility()
    {
        Modifier WeaponLengthPlus = new Modifier(StatType.WeaponLength, 3f);

        StartCoroutine(AddModifier(WeaponLengthPlus));

        MeleeWeapon.Init(EntityStats, Special.DamageMultiplier.GetFinalValue(), false);
    }

    public void UltimateAbility()
    {
        IsUltimateActive = true;
        StartCoroutine(MiscUtilities.Instance.ActionWithDelay(Ultimate.DurationInSecs, () => IsUltimateActive = false));
    }
}