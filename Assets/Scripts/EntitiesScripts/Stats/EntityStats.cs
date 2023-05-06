using Akaal.PvCustomizer.Scripts;
using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public enum StatType
{
    Health,
    Stamina,
    Mana, ManaConsumption, ManaRecoveryBonus,
    Attack,
    Defence,
    CritChance, CritDamage,
    InventorySlots,
    Speed,

    [InspectorName(null)]
    WeaponLength,

    Resistance, ElementalDamageBonus
}

public enum EntityType { Enemy, Character }

public enum WeaponType
{
    OneHandSword, TwoHandSword, Bow, Crossbow, Pistol, Book, Stave, Axe
}


[CreateAssetMenu(fileName = "Entity Stats", menuName = "Objects/Entity Stats")]
public class EntityStats : ScriptableObject, ICollectable, ILocalizable
{
#if UNITY_EDITOR

    public void OnValidate()
    {
        var list = Resources.LoadAll($"ScriptableObjects/{Type}", typeof(EntityStats)).Cast<EntityStats>().ToList().OrderBy(x => x.Name); ;
        this.id = list.ToList().IndexOf(this) + 1;

        if (this.Name == "Ardalion" | this.Name == "Marfa" | this.Name == "Dream")
            unlocked = true;

        foreach (var item in skills)
            item.SetName(this.Name);

        this._Description = Name + "_Desc";

        this.EnemyLayer = this.Type == EntityType.Enemy ? AIUtilities.CharsLayer : AIUtilities.EnemyLayer;
        this.EnemyTag = this.Type == EntityType.Enemy ? AIUtilities.CharsTag : AIUtilities.EnemyTag;

        this.EntityLayer = this.Type == EntityType.Character ? AIUtilities.CharsLayer : AIUtilities.EnemyLayer;
        this.EntityTag = this.Type == EntityType.Character ? AIUtilities.CharsTag : AIUtilities.EnemyTag;
    }

#endif

    [Foldout("Info", true)]
    [SerializeField]
    [ReadOnly]
    private int id;

    public int ID
    { get { return id - 1; } }

    public string Name => _Name;
    public string Description => _Description;

    [SerializeField]
    private string _Name;
    [ReadOnly]
    [SerializeField]
    private string _Description;

    public EntityType Type;
    public Rarity Rarity;

    [PvIcon]
    public Sprite Icon;

    public Sprite Art;

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
    public float Speed = 5;

    [HideInInspector]
    public int EnemyLayer, EntityLayer;

    [HideInInspector]
    public string EnemyTag, EntityTag;

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

    [HideInInspector]
    public float AttackSpeed, Sprint, DodgeCost, BlockCost, HeavyBlockCost;

    [Header("Abilities")]
    [SerializeField]
    public Skill[] skills = new Skill[3]
    {
        new Skill(SkillType.Special),
        new Skill(SkillType.Distract),
        new Skill(SkillType.Ultimate)
    };

    public Dictionary<SkillType, Skill> SkillSet;

    [Header("Attributes")]
    public Dictionary<StatType, Stat> ModifiableStats;

    public Modifier TeamBuff;

    public Stat WeaponLengthStat;

    [SerializeField]
    private bool unlocked = false;
    public bool IsUnlocked
    {
        get => unlocked;
        set => unlocked = value;
    }

    public virtual void Initialize(int Level)
    {
        if (this.Type == EntityType.Character)
        {
            SkillSet = new Dictionary<SkillType, Skill>(3)
            {
                { SkillType.Special, skills[0]},
                { SkillType.Distract, skills[1]},
                { SkillType.Ultimate, skills[2]},
            };

            foreach (var item in SkillSet)
            {
                Skill skill = item.Value;
                skill.DamageMultiplier = new Stat(skill.BaseDamageMultiplier, skill.Level, skill.LevelMod);
            }

            EnduranceStat = new Stat(Endurance);
            AgilityStat = new Stat(Agility);
        }

        ModifiableStats = new Dictionary<StatType, Stat>()
        {
            { StatType.Attack, new Stat(BaseAttack, Level, AttackMod)},
            { StatType.CritChance, new Stat(Luck * 1.5f, Level, CritChanceMod)},
            { StatType.CritDamage, new Stat(BaseCritDamage, Level, CritDamageMod)},
            { StatType.Defence, new Stat(BaseDefence, Level, DefenceMod)},
            { StatType.Health, new Stat(BaseHealth, Level, HealthMod)},
            { StatType.Mana, new Stat(BaseMana, Level, ManaMod)},
            { StatType.Speed, new Stat(5.5f + Agility * 0.6f) },
            { StatType.Stamina, new Stat(BaseStamina, Level, StaminaMod)},
            { StatType.WeaponLength, WeaponLengthStat },
            { StatType.ManaConsumption, new Stat() },
            { StatType.ManaRecoveryBonus, new Stat() }
        };

        ElementsResBonus = new Dictionary<Element, ElementSheet>
        {
            { Element.Air, new ElementSheet (new Stat(AirRes), new Stat()) },
            { Element.Dark, new ElementSheet(new Stat(DarkRes), new Stat()) },
            { Element.Earth, new ElementSheet (new Stat(EarthRes), new Stat()) },
            { Element.Fire, new ElementSheet (new Stat(FireRes), new Stat()) },
            { Element.Light, new ElementSheet(new Stat(LightRes), new Stat()) },
            { Element.Water, new ElementSheet (new Stat(WaterRes), new Stat()) }
        };

        //agent.angularSpeed = 200f;

        SetLevel(Level);
        UpdateStats();
    }

    public void UpdateStats()
    {
        AttackSpeed = ModifiableStats[StatType.Speed].GetFinalValue() / 6;
        Sprint = ModifiableStats[StatType.Speed].GetFinalValue() * 1.3f;
        Speed = ModifiableStats[StatType.Speed].GetFinalValue();

        if (this.Type == EntityType.Character)
        {
            DodgeCost = 35 - EnduranceStat.GetFinalValue() * 2;
            BlockCost = 35 - EnduranceStat.GetFinalValue() * 1.5f;
            HeavyBlockCost = BlockCost * 1.5f;
        }
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

        for (int i = 0; i < ModifiableStats.Count - 1; i++)
        {
            if (ModifiableStats.ElementAt(i).Value != null)
            {
                ModifiableStats.ElementAt(i).Value.SetLevel(Level);
            }
        }
    }

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
}