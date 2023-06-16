using Akaal.PvCustomizer.Scripts;
using MyBox;
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

    Resistance, ElementalDamageBonus
}

public enum EntityType { Enemy, Character }

public enum WeaponType
{
    OneHandSword, TwoHandSword, Bow, Crossbow, Pistol, Book, Stave, Axe
}


[CreateAssetMenu(fileName = "Entity stats", menuName = "Objects/Entity stats")]
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

    #region [Info]
    [Foldout("Info", true)]
    [SerializeField]
    [ReadOnly]
    private int id;

    public int ID => id - 1;

    [SerializeField]
    [ReadOnly]
    public int Level = 1;

    public string Name => _Name;
    public string Description => _Description;

    [SerializeField]
    private string _Name;
    [ReadOnly]
    [SerializeField]
    private string _Description;

    public EntityType Type;
    public Rarity Rarity;

    public Element Element;

    public WeaponType WeaponType;
    public bool IsHeavy;

    [PvIcon]
    public Sprite Icon;
    public Sprite Art;

    [SerializeField]
    [ReadOnly]
    private bool unlocked = false;
    public bool IsUnlocked
    {
        get => unlocked;
        set => unlocked = value;
    }
    #endregion

    #region [Attributes]
    public Stat Health, Attack, Defence;
    public Stat Mana, ManaRecoveryBonus, ManaConsumption;

    [HideInInspector]
    public Stat FireRes, WaterRes, EarthRes, AirRes, LightRes, DarkRes;
    [HideInInspector]
    public Stat FireBonus, WaterBonus, EarthBonus, AirBonus, LightBonus, DarkBonus;

    public Dictionary<StatType, Stat> ModifiableStats;
    public Dictionary<Element, ElementSheet> ElementsResBonus; 
    #endregion

    #region [AI]
    [Header("AI")]
    public float SightRange = 20f;

    public float AttackRange = 3f;
    [HideInInspector]
    public float MovementSpeed = 5;

    [HideInInspector]
    public int EnemyLayer, EntityLayer;

    [HideInInspector]
    public string EnemyTag, EntityTag; 
    #endregion

    [Foldout("Character", true)]
    public Stat CritDamage, CritChance;

    public Stat Endurance, Agility, Luck;

    public Stat Stamina, Speed;

    [HideInInspector]
    public float AttackSpeed, Sprint, DodgeCost, BlockCost, HeavyBlockCost;

    [Header("Skills")]
    public Skill NormalAttackSkill;
    [SerializeField]
    public Skill[] skills = new Skill[3]
    {
        new Skill(SkillType.Special),
        new Skill(SkillType.Distract),
        new Skill(SkillType.Ultimate)
    };

    public Dictionary<SkillType, Skill> SkillSet;

    public Modifier TeamBuff;

    [HideInInspector]
    public Stat WeaponLengthStat;

    public void Initialize(int Level)
    {
        if (this.Type == EntityType.Character)
        {
            SkillSet = new Dictionary<SkillType, Skill>(3)
            {
                { SkillType.Special, skills[0]},
                { SkillType.Distract, skills[1]},
                { SkillType.Ultimate, skills[2]},
                { SkillType.NormalAttack, NormalAttackSkill}
            };
        }

        ModifiableStats = new Dictionary<StatType, Stat>()
        {
            { StatType.Attack, Attack },
            
            { StatType.Defence, Defence },
            { StatType.Health, Health },
            { StatType.Mana, Mana },
            { StatType.ManaConsumption, ManaConsumption },
            { StatType.ManaRecoveryBonus, ManaRecoveryBonus },
            { StatType.Speed, Speed },
            { StatType.Stamina, Stamina },
            { StatType.CritChance, CritChance },
            { StatType.CritDamage, CritDamage },
        };

        ElementsResBonus = new Dictionary<Element, ElementSheet>
        {
            { Element.Air, new ElementSheet (AirRes, AirBonus) },
            { Element.Dark, new ElementSheet(DarkRes, DarkBonus) },
            { Element.Earth, new ElementSheet (EarthRes, EarthBonus) },
            { Element.Fire, new ElementSheet (FireRes, FireBonus) },
            { Element.Light, new ElementSheet(LightRes, LightBonus) },
            { Element.Water, new ElementSheet (WaterRes, WaterBonus) }
        };

        SetLevel(Level);
        UpdateStats();
    }

    public void UpdateStats()
    {
        AttackSpeed = ModifiableStats[StatType.Speed].GetFinalValue() / 6;
        Sprint = ModifiableStats[StatType.Speed].GetFinalValue() * 1.3f;
        MovementSpeed = ModifiableStats[StatType.Speed].GetFinalValue();

        if (this.Type == EntityType.Character)
        {
            DodgeCost = 35 - Endurance.GetFinalValue() * 2;
            BlockCost = 35 - Endurance.GetFinalValue() * 1.5f;
            HeavyBlockCost = BlockCost * 1.5f;
        }
    }

    public void Clear() => ModifiableStats.ForEach(x => x.Value.ClearMods());

    public void LevelUp(int LevelAmount)
    {
        this.Level += LevelAmount;

        foreach (var item in ModifiableStats)
        {
            item.Value.NewLevel(LevelAmount);
        }
    }

    public void SetLevel(int Level)
    {
        this.Level = Level;

        for (int i = 0; i < ModifiableStats.Count - 1; i++)
            ModifiableStats.ElementAt(i).Value?.SetLevel(Level);
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