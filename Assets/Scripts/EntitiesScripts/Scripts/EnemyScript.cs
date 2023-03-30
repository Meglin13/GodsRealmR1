using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : EntityScript
{
    #region [Enemy Stats]

    [HideInInspector]
    public IEnemy Enemy;

    public override void Initialize()
    {
        base.Initialize();

        AttackRange = EntityStats.AttackRange;
        SightRange = EntityStats.SightRange;

        EntityStateMachine = new StateMachine();

        IdleState idleState = new IdleState(gameObject, EntityStateMachine);

        EntityStateMachine.Initialize(idleState);
    }

    #endregion [Enemy Stats]

    #region [IDamageable and Health]

    public override void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry)
    {
        float damage = (int)Math.Floor(CombatManager.DamageCalc(EntityStats, DealerStats, Multiplier));
        string color = GameManager.Instance.colorManager.ElementColor[DealerStats.Element];
        float scale = 1;

        System.Random random = new();
        double chance = (random.NextDouble() * (100 - 1) + 1);
        if (chance <= DealerStats.ModifiableStats[StatType.CritChance].GetFinalValue())
        {
            damage += damage * DealerStats.ModifiableStats[StatType.CritDamage].GetFinalValue() / 100;
            scale = 2f;
        }

        CurrentHealth -= damage;

        MiscUtilities.DamagePopUp(transform, damage.ToString(), color, scale);

        base.TakeDamage(DealerStats, Multiplier, CanParry);
    }

    public override void Stun(float Time)
    {
        base.Stun(Time);
        EntityStateMachine.ChangeState(new StunnedState(gameObject, EntityStateMachine, Time));
    }

    public override void Death()
    {
        base.Death();
        EntityStateMachine.ChangeState(new DyingState(gameObject, EntityStateMachine));
        //TODO: Скорректировать количество маны с врага
        GameManager.Instance.partyManager.GiveSupportToAll((int)(EntityStats.Rarity + 1) * 10, StatType.Mana);
    }


    internal void SpawnHealthBar()
    {
        var HealthBarPrefab = GameObject.Instantiate(GameManager.Instance.HealthBar, gameObject.transform, false);
        HealthBarPrefab.transform.localPosition = new Vector3(0, 2.5f, 0);
        HealthBarPrefab.transform.localScale *= 0.25f;
        HealthBar = HealthBarPrefab.GetComponentInChildren<BarScript>();

        OnTakeDamage += () => HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
        HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
    }

    #endregion [IDamageable and Health]

    #region [AI]

    [Header("AI")]
    [HideInInspector]
    public StateMachine EntityStateMachine;

    public string CurrentState, TargetName;

    public bool IsCharInAttackRange, IsCharInSightRange;
    private float AttackRange, SightRange;

    #endregion [AI]

    #region [Unity Methods]

    public override void Awake()
    {
        base.Awake();
    }

    public virtual void Start()
    {
        SpawnHealthBar();

        GameManager.Instance.miniMapManager.Initialize();
    }

    public virtual void Update()
    {

    }

    //private void FixedUpdate()
    //{
    //    EntityStateMachine.CurrentState.PhysicsUpdate();
    //}

    #endregion [Unity Methods]

    #region [Behavior Patterns]

    //TODO: Паттерны поведения врагов переделать в новый ИИ
    internal void FollowAndAttack()
    {
        GameObject target = AIUtilities.FindNearestEntity(transform, EnemyTag);
        if (target)
        {
            TargetName = target.ToString();
            CurrentState = EntityStateMachine.CurrentState.GetType().Name;

            IsCharInAttackRange = AIUtilities.IsCertainEntityInRadius(gameObject, target, AttackRange);
            IsCharInSightRange = AIUtilities.IsCertainEntityInRadius(gameObject, target, SightRange);

            if (!IsCharInAttackRange & IsCharInSightRange)
                EntityStateMachine.ChangeState(new TargetFollowingState(target, gameObject, EntityStateMachine));
            else if (IsCharInAttackRange & IsCharInSightRange)
                EntityStateMachine.ChangeState(new AttackingState(this, EntityStateMachine));
            else
                EntityStateMachine.ChangeState(new IdleState(gameObject, EntityStateMachine));

            EntityStateMachine.CurrentState.LogicUpdate();
        }
        else
            Debug.Log("No characters found!");
    }

    #endregion [Behavior Patterns]
}