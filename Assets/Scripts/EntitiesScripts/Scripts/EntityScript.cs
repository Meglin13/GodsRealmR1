using MyBox;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[SelectionBase]
public abstract class EntityScript : MonoBehaviour, IDamageable
{
    [MustBeAssigned]
    [SerializeField]
    private EntityStats Stats;

    [Range(1, 100)]
    public int Level = 1;

    [HideInInspector]
    public MeleeWeapon MeleeWeapon;

    [HideInInspector]
    public Animator animator;

    [HideInInspector]
    internal CinemachineImpulseSource impulseSource;

    internal BarScript HealthBar;

    public virtual void Initialize()
    {
        this.EntityStats = Stats;
        EntityStats.Initialize(Level);

        CurrentHealth = EntityStats.ModifiableStats[StatType.Health].GetFinalValue();

        if (EntityStats.Type == EntityType.Enemy)
        {
            EntityStats.EnemyLayer = EnemyLayer = AIUtilities.CharsLayer;
            EntityStats.EnemyTag = EnemyTag = AIUtilities.CharsTag;

            gameObject.tag = AIUtilities.EnemyTag;
            gameObject.layer = AIUtilities.EnemyLayer;
        }
        else
        {
            EntityStats.EnemyLayer = EnemyLayer = AIUtilities.EnemyLayer;
            EntityStats.EnemyTag = EnemyTag = AIUtilities.EnemyTag;

            gameObject.tag = AIUtilities.CharsTag;
            gameObject.layer = AIUtilities.CharsLayer;
        }

        //Collider
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.height = 2;
        collider.center = new Vector3(0, 1, 0);

        //RigidBody
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

        //MeleeWeapon
        MeleeWeapon = GetComponentInChildren<MeleeWeapon>();

        if (MeleeWeapon)
        {
            EntityStats.WeaponLengthStat = MeleeWeapon.WeaponLength;
        }

        //Animator
        animator = GetComponent<Animator>();

        //NavMesh
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.speed = 5f;
        agent.stoppingDistance = 2f;

        //Cinemachine
        impulseSource = GetComponent<CinemachineImpulseSource>();

        OnAddModifier += () => EntityStats.UpdateStats();
    }

    #region [Delegates]

    public event Action OnAddModifier = delegate { };
    public event Action OnDie = delegate { };
    public event Action OnStun = delegate { };
    public event Action OnTakeDamage = delegate { };
    public event Action OnManaChange = delegate { };
    public event Action OnHeal = delegate { };
    public event Action OnLevelChange = delegate { };

    #endregion [Delegates]

    #region [IDamageable]
    public float CurrentHealth { get; set; }
    public EntityStats EntityStats { get; set; }
    public int EnemyLayer { get; set; }
    public string EnemyTag { get; set; }

    public virtual void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry)
    {
        if (CurrentHealth <= 0)
            Death();

        impulseSource.GenerateImpulse();

        OnTakeDamage();
    }

    public virtual void Stun(float Time)
    {
        OnStun();
        StartCoroutine(AddModifier(new Modifier(StatType.Defence, 20, ModifierAmountType.Procent, 5f, ModType.Debuff)));
    }

    public virtual void Death() => OnDie();

    public void HitVFX(Vector3 hitPosition)
    {
        var hitVFX = GameManager.Instance.HitVFX;
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 0.5f);
    }
    #endregion


    #region [Unity Methods]
    public virtual void Awake()
    {
        Initialize();
    }

    #endregion

    #region[Utilities]
    public void UpdateHealthBar()
    {
        HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
    }

    public virtual void GiveSupport(float Amount, StatType type)
    {
        switch (type)
        {
            case StatType.Health:
                Debug.Log("Bruh");
                CurrentHealth = Mathf.Clamp(CurrentHealth + Amount, 0, EntityStats.ModifiableStats[type].GetFinalValue());
                OnHeal();
                break;

            case StatType.Mana:
                OnManaChange();
                break;
        }
    }

    public void StartDealMeleeDamage()
    {
        MeleeWeapon.enabled = true;
        MeleeWeapon.Init(EntityStats, EntityStats.NormalAttackMult, true);
    }

    public void EndDealMeleeDamage() => MeleeWeapon.EndDealDamage();

    public IEnumerator AddModifier(Modifier modifier)
    {
        if (modifier != null)
        {
            OnAddModifier();

            //Debug.Log($"Added modifier {modifier.AmountOfPotion} for {modifier.DurationInSecs} sec. Stat before modifier {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");
            
            if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
            {
                if (modifier.StatType == StatType.Resistance)
                {
                    this.EntityStats.ElementsResBonus[modifier.Element].Resistance.AddModifier(modifier);
                }
                else
                {
                    this.EntityStats.ElementsResBonus[modifier.Element].DamageBonus.AddModifier(modifier);
                }
            }
            else
                this.EntityStats.ModifiableStats[modifier.StatType].AddModifier(modifier);

            //Debug.Log($"Stat after modifier {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");

            yield return new WaitForSeconds(modifier.DurationInSecs);

            if (!modifier.IsPermanent)
            {
                if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
                {
                    if (modifier.StatType == StatType.Resistance)
                    {
                        this.EntityStats.ElementsResBonus[modifier.Element].Resistance.RemoveModifier(modifier);
                    }
                    else
                    {
                        this.EntityStats.ElementsResBonus[modifier.Element].DamageBonus.RemoveModifier(modifier);
                    }
                }
                else
                    this.EntityStats.ModifiableStats[modifier.StatType].RemoveModifier(modifier);

                //Debug.Log($"Removed modifier. Current stat value {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");
            }
        }
        else
            throw new Exception("Modifier is NULL");
    }

    #endregion
}
