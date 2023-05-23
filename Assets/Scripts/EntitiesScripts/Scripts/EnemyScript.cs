using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : EntityScript
{
    #region [Enemy Stats]

    [HideInInspector]
    public IEnemy Enemy;

    public Collider SightTrigger;


    public override void Initialize()
    {
        base.Initialize();

        AttackRange = EntityStats.AttackRange;
        SightRange = EntityStats.SightRange;

        EntityStateMachine = new StateMachine();

        IdleState idleState = new IdleState(gameObject, EntityStateMachine);

        EntityStateMachine.Initialize(idleState);

        SpawnHealthBar();
    }

    #endregion [Enemy Stats]

    #region [IDamageable and Health]

    System.Random random = new();
    public override void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanParry)
    {
        if (CurrentHealth > 0)
        {
            float Damage = Mathf.FloorToInt(CombatManager.DamageCalc(EntityStats, DealerStats, Multiplier));

            string color = GameManager.Instance.colorManager.ElementColor[DealerStats.Element];
            float scale = 1;
            
            double chance = (random.NextDouble() * (100 - 1) + 1);
            if (chance <= DealerStats.ModifiableStats[StatType.CritChance].GetFinalValue())
            {
                Damage += Mathf.FloorToInt(Damage * DealerStats.ModifiableStats[StatType.CritDamage].GetFinalValue() / 100);
                scale = 2f;
            }

            CurrentHealth -= Damage;

            MiscUtilities.DamagePopUp(transform, Damage.ToString(), color, scale);

            base.TakeDamage(DealerStats, Multiplier, CanParry); 
        }
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

        GameManager.Instance.partyManager.GiveSupportToAll((int)(EntityStats.Rarity + 1) * 7, StatType.Mana);
        InventoryScript.Instance.Score += (int)(EntityStats.Rarity + 1) * 10;
    }

    internal void SpawnHealthBar()
    {
        var HealthBarPrefab = Instantiate(GameManager.Instance.HealthBar, gameObject.transform, false);
        HealthBarPrefab.transform.localPosition = new Vector3(0, 2.5f, 0);
        HealthBarPrefab.transform.localScale *= 0.25f;
        HealthBar = HealthBarPrefab.GetComponentInChildren<BarScript>();

        HealthBar.SetLevel(EntityStats.Level);

        OnTakeDamage -= UpdateHealthBar;
        OnTakeDamage += UpdateHealthBar;
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

    public virtual void Start()
    {
        GameManager.Instance.miniMapManager.Initialize();
    }

    private void OnEnable()
    {
        CurrentHealth = 1000;
        UpdateHealthBar();
        HealthBar.SetLevel(EntityStats.Level);
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
        var targetT = AIUtilities.GetTarget(transform, EntityStats);

        if (targetT != null)
        {
            var target = targetT.gameObject;

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
    }

    #endregion [Behavior Patterns]

    #region [Utilities]

    #endregion
}