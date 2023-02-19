using MyBox;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum SkillsType { Special = 0, Distract = 1, Ultimate = 2}

[Serializable]
public class Skill
{
    public byte Level = 1;
    public float LevelMod = 10f;
    public string SkillName = "Skill Name";
    public string Description = "Generic Description";
    [ReadOnly]
    public SkillsType SkillType;
    public Sprite SkillIcon;
    public float BaseDamageMultiplier = 100f;
    public Stat DamageMultiplier;
    public float CooldownInSecs = 5f;
    private bool Cooldown = false;
    public float DurationInSec = 0f;
    public float ManaCost = 10f;
    public float Radius = 1f;
    [SerializeField]
    private bool IsDebuffing = false;
    [ConditionalField("IsDebuffing")]
    public Modifier EnemyModifier;
    [ConditionalField("IsDebuffing")]
    public float StunTime = 0;
    public event Action OnSkillTrigger = delegate { };

    public void ActivateSkill()
    {
        OnSkillTrigger();
        SetCooldown();
    }

    public void SetCooldown() => Cooldown = true;
    public bool IsCooldown() => Cooldown;
    public void ResetCooldown() => Cooldown = false;

    public void LevelUpSkill(byte newLevel) => Level += newLevel;
}