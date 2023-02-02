using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum StatType{Health, Attack, Mana, Defence, CritChance, CritDamage, InventorySlots, Stamina, Speed, WeaponLength}
public enum EntityType { Enemy, Character }

public class ElementSheet
{
    public ElementSheet(Stat Res, Stat Bonus)
    {
        Resistance = Res;
        DamageBonus = Bonus;
    }
    public Stat Resistance { get; set; }
    public Stat DamageBonus { get; set; }
}

[CreateAssetMenu(fileName = "EntityStats", menuName = "Objects/Entity Stats")]
public class EntityStats : ScriptableObject
{
    [Foldout("Info", true)]
    public string Name, Description;
    public EntityType Type;
    public Sprite Icon, Art;
    public Rarity Rarity;

    [Foldout("Base", true)]

    [HideInInspector]
    public int Level = 1;

    [Header("Base Stats")]
    public Element Element;
    public WeaponType WeaponType;
    public bool IsHeavy;

    public float BaseHealth = 100;
    public float HealthMod = 10f;

    public float BaseAttack = 10;
    public float AttackMod = 10f;

    public float BaseDefence = 20;
    public float DefenceMod = 10f;

    [Header("AI")]
    public float SightRange = 20f;
    public float AttackRange = 3f;
    public float AttackCooldown = 5f;
    public float Speed = 6;

    public Dictionary<Element, ElementSheet> ElementsResBonus;

    public float FireRes, WaterRes, EarthRes, AirRes, LightRes, DarkRes;

    [Header("Abilities Multipliers")]
    public float NormalAttackMult = 70f;

    ///////////////////////////////////////////////////

    [Foldout("Character", true)]

    //Криты и удача
    [Header("Luck and Critical")]
    [Range(1, 10)]
    public int Luck = 1;
    [Min(100)]
    public float BaseCritDamage = 150f;
    public float CritChanceMod = 0;
    public float CritDamageMod = 0;

    //Мана
    [Header("Mana")]
    public float BaseMana = 100f;
    public float ManaMod = 0;
    public float ManaRecoveringBonus = 10f;

    //Характеристики связанные с выносливостью и ловкостью
    [Header("Attributes")]
    [SerializeField]
    [Range(1, 10)]
    private int Endurance = 1;
    public Stat EnduranceStat;
    [SerializeField]
    [Range(1, 10)]
    private int Agility = 1;
    public Stat AgilityStat;

    [Header("Stamina and BaseSpeed")]
    public float BaseStamina = 100f;
    public float StaminaMod = 0;
    [HideInInspector] public float AttackSpeed, Sprint, DodgeCost, BlockCost, HeavyBlockCost;
    //TODO: Сделать зависимость блока от других параметров

    [Header("Abilities")]
    [SerializeField]
    private Skill[] skills = new Skill[3]
    {
        new Skill() { SkillType = SkillsType.Special },
        new Skill() { SkillType = SkillsType.Distract },
        new Skill() { SkillType = SkillsType.Ultimate }
    };

    public Dictionary<SkillsType, Skill> SkillSet;

    [Header("Attributes")]
    public Dictionary<StatType, Stat> ModifiableStats;
    public Modifier TeamBuff;

    public Stat WeaponLengthStat;

    public virtual void Init(int Level)
    {
        SkillSet = new Dictionary<SkillsType, Skill>(3)
        {
            { SkillsType.Special, skills[0]},
            { SkillsType.Distract, skills[1]},
            { SkillsType.Ultimate, skills[2]},
        };

        foreach (var item in SkillSet)
        {
            Skill skill = item.Value;
            skill.DamageMultiplier = new Stat(skill.BaseDamageMultiplier, skill.Level, skill.LevelMod);
        }

        EnduranceStat = new Stat(Endurance);
        AgilityStat = new Stat(Agility);

        ModifiableStats = new Dictionary<StatType, Stat>()
        {
            { StatType.Attack, new Stat(BaseAttack, Level, AttackMod)},
            { StatType.Health, new Stat(BaseHealth, Level, HealthMod)},
            { StatType.Defence, new Stat(BaseDefence, Level, DefenceMod)},
            { StatType.Stamina, new Stat(BaseStamina, Level, StaminaMod)},
            { StatType.CritChance, new Stat(Luck * 1.5f, Level, CritChanceMod)},
            { StatType.CritDamage, new Stat(BaseCritDamage, Level, CritDamageMod)},
            { StatType.Mana, new Stat(BaseMana, Level, ManaMod)},
            { StatType.Speed, new Stat(5.5f + Agility * 0.6f) },
            { StatType.WeaponLength, WeaponLengthStat }
        };

        ElementsResBonus = new Dictionary<Element, ElementSheet>
        {
            { Element.Fire, new ElementSheet (new Stat(FireRes), new Stat()) },
            { Element.Water, new ElementSheet (new Stat(WaterRes), new Stat()) },
            { Element.Earth, new ElementSheet (new Stat(EarthRes), new Stat()) },
            { Element.Air, new ElementSheet (new Stat(AirRes), new Stat()) },
            { Element.Light, new ElementSheet(new Stat(LightRes), new Stat()) },
            { Element.Dark, new ElementSheet(new Stat(DarkRes), new Stat()) }
        };

        //agent.angularSpeed = 200f;

        SetLevel(Level);
        UpdateStats();
    }

    public void UpdateStats()
    {
        AttackSpeed = ModifiableStats[StatType.Speed].GetFinalValue() / 6;
        Sprint = ModifiableStats[StatType.Speed].GetFinalValue() * 1.5f;
        Speed = ModifiableStats[StatType.Speed].GetFinalValue();

        DodgeCost = 35 - EnduranceStat.GetFinalValue() * 2;
        BlockCost = 35 - EnduranceStat.GetFinalValue() * 1.5f;
        HeavyBlockCost = BlockCost * 1.5f;
    }

    public void LevelUp(int LevelAmount)
    {
        this.Level = LevelAmount;

        foreach (var item in ModifiableStats)
        {
            item.Value.NewLevel(LevelAmount);
        }
    }

    public void SetLevel(int Level)
    {
        this.Level = Level;

        for (int i = 0; i < ModifiableStats.Count-1; i++)
        {
            ModifiableStats.ElementAt(i).Value.SetLevel(Level);
        }

        //foreach (var item in ModifiableStats)
        //{
        //    Debug.Log(item.Value);
        //    item.Value.SetLevel(Level);
        //}
    }
}
