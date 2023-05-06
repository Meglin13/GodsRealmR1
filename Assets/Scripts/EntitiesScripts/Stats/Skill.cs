using MyBox;
using System;
using UnityEngine;

public enum SkillType { Special = 0, Distract = 1, Ultimate = 2 }

[Serializable]
public class Skill : ILocalizable
{
    public byte Level = 1;
    public float LevelMod = 10f;

    public string Name => SkillName;
    public string Description => _Description;

    [ReadOnly]
    [SerializeField]
    private string SkillName;
    [ReadOnly]
    [SerializeField]
    private string _Description;

    [ReadOnly]
    public SkillType SkillType;
    public Sprite SkillIcon;

    public float BaseDamageMultiplier = 100f;
    public Stat DamageMultiplier;

    public SpawningObject SpawningObject;

    public float CooldownInSecs = 5f;
    private bool Cooldown = false;

    public float DurationInSecs = 0f;
    public bool IsPeriodic = false;
    [ConditionalField(nameof(IsPeriodic))]
    public float PeriodicDamageTime;

    public float ManaCost = 10f;
    public float Radius = 1f;

    [SerializeField]
    private bool IsDebuffing = false;
    [ConditionalField("IsDebuffing")]
    public Modifier EnemyModifier;
    [ConditionalField("IsDebuffing")]
    public float StunTime = 0;

    public event Action OnSkillTrigger = delegate { };

    public Skill(SkillType type)
    {
        this.SkillType = type;
    }

    public Skill(float radius, float baseDamage)
    {
        this.Radius = radius;
        this.BaseDamageMultiplier = baseDamage;
        DamageMultiplier = new Stat(BaseDamageMultiplier);
    }

    public void SetName(string name)
    {
        var type = SkillType;

        this.SkillName = $"{name}_{type}";
        this._Description = $"{name}_{type}_Desc";
    }

    public void ActivateSkill()
    {
        SetCooldown();
        OnSkillTrigger();
    }

    public void ClearEvents() => ObjectsUtilities.UnsubscribeEvents(OnSkillTrigger);

    public void SetCooldown() => Cooldown = true;
    public bool IsCooldown() => Cooldown;
    public void ResetCooldown() => Cooldown = false;

    public void LevelUpSkill(byte newLevel) => Level += newLevel;
}