using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

[SelectionBase]
public class EnemyScript : MonoBehaviour, IDamageable
{
    //Статы
    #region [Enemy Stats]
    [Header("Enemy Stats")]
    [Range(1, 100)]
    public int Level = 1;
    public EntityStats EnemyStats;

    [HideInInspector]
    public IEnemy Enemy;

    [HideInInspector]
    public MeleeWeapon Weapon;

    private void InitiateEnemyStats()
    {
        gameObject.tag = AIUtilities.EnemyTag;
        gameObject.layer = AIUtilities.EnemyLayer;

        EntityStats = EnemyStats;

        EntityStats.Init(Level);

        MaxHealth = EntityStats.ModifiableStats[StatType.Health].GetFinalValue();
        CurrentHealth = MaxHealth;

        AttackRange = EntityStats.AttackRange;
        SightRange = EntityStats.SightRange;

        EntityStateMachine = new StateMachine();

        IdleState idleState = new IdleState(gameObject, EntityStateMachine);

        EntityStateMachine.Initialize(idleState);

        Weapon = GetComponentInChildren<MeleeWeapon>();

        //Collider
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.height = 2;
        collider.center = new Vector3(0, 1, 0);

        //RigidBody
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
    }
    #endregion

    #region [IDamageable and Health]
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public EntityStats EntityStats { get; set; }

    public void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry)
    {
        //gameObject.GetComponent<Animator>().SetTrigger("TakeDamage");

        float Damage = (int)Math.Floor(CombatManager.DamageCalc(EntityStats, DealerStats, Multiplier));

        string color = "#" + GameManager.Instance.colorManager.ElementColor[DealerStats.Element];

        float scale = 1;

        System.Random random = new();
        double chance = (random.NextDouble() * (100 - 1) + 1);
        if (chance <= DealerStats.ModifiableStats[StatType.CritChance].GetFinalValue())
        {
            Damage += Damage * DealerStats.ModifiableStats[StatType.CritDamage].GetFinalValue() / 100;
            scale = 1.5f;
        }

        CurrentHealth -= Damage;

        MiscUtilities.DamagePopUp(transform, Damage.ToString(), color, scale);

        if (CurrentHealth <= 0)
            Death();

        UpdateHealthBar();
    }

    public void Stun(float Time)
    {
        StunnedState StunnedState = new StunnedState(gameObject, EntityStateMachine, Time);
        EntityStateMachine.ChangeState(StunnedState);
    }

    public virtual void Death()
    {
        EntityStateMachine.ChangeState(new DyingState(gameObject, EntityStateMachine));
    }

    private BarScript HealthBar;

    internal void SpawnHealthBar()
    {
        var HealthBarPrefab = GameObject.Instantiate(GameManager.Instance.HealthBar, gameObject.transform, false);
        HealthBarPrefab.transform.localPosition = new Vector3(0, 2.5f, 0);
        HealthBarPrefab.transform.localScale *= 0.25f;
        HealthBar = HealthBarPrefab.GetComponentInChildren<BarScript>();
    }

    public void UpdateHealthBar()
    {
        HealthBar.ChangeBarValue(CurrentHealth , MaxHealth);
    }
    #endregion

    #region [AI]
    [Header("AI")]
    [HideInInspector]
    public StateMachine EntityStateMachine;
    public string CurrentState, TargetName;

    public bool IsCharInAttackRange, IsCharInSightRange;
    private float AttackRange, SightRange;

    #endregion

    public virtual void Start()
    {
        InitiateEnemyStats();
        SpawnHealthBar();

        GameManager.Instance.miniMapManager.Initialize();
    }

    public virtual void Update()
    {

    }

    //private void FixedUpdate()
    //{
    //    CharacterStateMachine.CurrentState.PhysicsUpdate();
    //}

    #region [Behavior Patterns]
    //TODO: Паттерны поведения врагов переделать под стратегии
    internal void FollowAndAttack()
    {
        //EXP: NullRef при отсутствии персонажей. УБРАТЬ ОБРАБОТКУ ИСКЛЮЧЕНИЙ
        GameObject target = null;
        try
        {
            target = AIUtilities.FindNearestEntity(transform, AIUtilities.CharsTag);
            TargetName = target.ToString();
            CurrentState = EntityStateMachine.CurrentState.GetType().Name;

            IsCharInAttackRange = AIUtilities.IsCertainEntityInRadius(gameObject, target, AttackRange);
            IsCharInSightRange = AIUtilities.IsCertainEntityInRadius(gameObject, target, SightRange);

            if (!IsCharInAttackRange & IsCharInSightRange)
                EntityStateMachine.ChangeState(new TargetFollowingState(target, gameObject, EntityStateMachine));
            else if (IsCharInAttackRange & IsCharInSightRange)
                EntityStateMachine.ChangeState(new AttackingState(gameObject, EntityStateMachine));
            else
                EntityStateMachine.ChangeState(new IdleState(gameObject, EntityStateMachine));

            EntityStateMachine.CurrentState.LogicUpdate();
        }
        catch (System.Exception)
        {
            Debug.Log("No characters found!");
        }
    }

    internal void AvoidAndAttack()
    {

    }
    #endregion

    #region [Combat]
    public void HitVFX(Vector3 hitPosition)
    {
        var hitVFX = GameManager.Instance.HitVFX;

        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 1f);
    }

    public void StartDealMeleeDamage()
    {
        Weapon.StartDealDamage();
        Weapon.Init(EntityStats, EntityStats.NormalAttackMult, true);
    }

    public void EndDealMeleeDamage() => Weapon.EndDealDamage();
    #endregion
}