using Cinemachine;
using MyBox;
using ObjectPooling;
using System;
using System.Collections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CinemachineImpulseSource))]
[SelectionBase]
public abstract class EntityScript : MonoBehaviour, IDamageable
{
#if UNITY_EDITOR

    public virtual void OnValidate()
    {
        if (Stats)
        {
            GetComponent<Animator>().runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>($"AnimatorControllers/{EntityStats.Type}/{EntityStats.Name}");

            //Collider
            CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
            collider.height = 2;
            collider.center = new Vector3(0, 1, 0);

            //RigidBody
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.freezeRotation = true;

            //NavMesh
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.speed = 5f;
            agent.stoppingDistance = 2f;

            gameObject.tag = EntityStats.EntityTag;
            gameObject.layer = EntityStats.EntityLayer;

            EntityStats.EntityLayer = gameObject.layer;

            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.layer = gameObject.layer;
            }
        }
    }

#endif

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
    public NavMeshAgent agent;

    [HideInInspector]
    internal CinemachineImpulseSource impulseSource;

    internal BarScript HealthBar;

    public virtual void Initialize()
    {
        EntityStats.Initialize(Level);

        CurrentHealth = EntityStats.ModifiableStats[StatType.Health].GetFinalValue();

        //MeleeWeapon
        MeleeWeapon = GetComponentInChildren<MeleeWeapon>();

        if (MeleeWeapon)
            EntityStats.WeaponLengthStat = MeleeWeapon.WeaponLength;

        //Animator
        animator = GetComponent<Animator>();

        //NavMesh
        agent = GetComponent<NavMeshAgent>();

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
    private float health;
    public float CurrentHealth { 
        get => health; 
        set => health = Mathf.Clamp(value, 0, EntityStats.ModifiableStats[StatType.Health].GetFinalValue()); }
    public EntityStats EntityStats { get => Stats; }
    public int EnemyLayer { get => EntityStats.EnemyLayer; }
    public string EnemyTag { get => EntityStats.EnemyTag; }

    public virtual void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry)
    {
        if (CurrentHealth <= 0)
            Death();

        OnTakeDamage();
    }

    public virtual void Stun(float Time)
    {
        OnStun();
        StartCoroutine(AddModifier(new Modifier(StatType.Defence, 20, ModifierAmountType.Procent, Time, ModType.Debuff)));
    }

    public virtual void Death() => OnDie();

    public void HitVFX(Vector3 hitPosition) => VFXPool.Instance.CreateObject(GameManager.Instance.HitVFX, hitPosition);

    #endregion [IDamageable]

    #region [Unity Methods]

    public virtual void Awake()
    {
        Initialize();
    }

    public virtual void OnDestroy()
    {
        Action[] delList = new Action[]
        {
            OnAddModifier,
            OnDie,
            OnStun,
            OnTakeDamage,
            OnManaChange,
            OnHeal,
            OnLevelChange
        };

        ObjectsUtilities.UnsubscribeEvents(delList);
    }

    private void OnEnable()
    {
        ResetAgent();
    }

    #endregion [Unity Methods]

    #region[Utilities]

    public void ResetAgent()
    {
        agent.enabled = false;
        agent.enabled = true;
    }

    public void UpdateHealthBar()
    {
        HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
    }

    public virtual void GiveSupport(float Amount, StatType type)
    {
        switch (type)
        {
            case StatType.Health:
                CurrentHealth += Amount;
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

            //Debug.Log($"Added modifier {modifier.StatType} {modifier.Amount} for {modifier.DurationInSecs} sec. Stat before modifier {EntityStats.ModifiableStats[modifier.StatType].GetFinalValue()}");

            if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
            {
                if (modifier.StatType == StatType.Resistance)
                    this.EntityStats.ElementsResBonus[modifier.Element].Resistance.AddModifier(modifier);
                else
                    this.EntityStats.ElementsResBonus[modifier.Element].DamageBonus.AddModifier(modifier);
            }
            else
                this.EntityStats.ModifiableStats[modifier.StatType].AddModifier(modifier);

            //Debug.Log($"Stat after modifier {EntityStats.ModifiableStats[modifier.StatType].GetFinalValue()}");

            yield return new WaitForSeconds(modifier.DurationInSecs);

            if (!modifier.IsPermanent)
            {
                if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
                {
                    if (modifier.StatType == StatType.Resistance)
                        this.EntityStats.ElementsResBonus[modifier.Element].Resistance.RemoveModifier(modifier);
                    else
                        this.EntityStats.ElementsResBonus[modifier.Element].DamageBonus.RemoveModifier(modifier);
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