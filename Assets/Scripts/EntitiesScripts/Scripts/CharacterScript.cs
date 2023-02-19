using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[SelectionBase]
public abstract class CharacterScript : EntityScript
{
    #region [Party Setup Settings]

    [Header("Party Setup Settings")]
    public int HotKeyNumber;
    public bool IsActive;

    #endregion [Party Setup Settings]

    #region [Character Script Stats]

    //Статы персонажа
    [Header("Character Stats")]
    [HideInInspector]
    public float blockingTime;
    private Camera _camera;

    //Интерфейс персонажа
    internal ICharacter Character;

    //Текущее состояние атрибутов
    public float CurrentStamina;
    public float CurrentMana;

    [HideInInspector]
    public bool IsBlocking, IsDodging;

    //Скорость и стамина
    [Header("BaseSpeed and Stamina")]
    [HideInInspector]
    public float velocity = 0f;

    public event Action OnStaminaChange = delegate { };
    public event Action OnItemEquip = delegate { };

    [HideInInspector]
    public float acceleration = 0.2f;

    [HideInInspector]
    public float Speed, Sprint, AttackSpeed;
    private float RotationSpeed = 600f;

    [HideInInspector]
    public float StaminaConsumption = 10f;

    [HideInInspector]
    public bool IsStaminaRecovering;

    //Объекты Unity
    [HideInInspector]
    public BarScript ManaBar, StaminaBar;

    private InputAction switchCharacterAction;
    private InputAction specialAbilityAction;
    private InputAction distractAbilityAction;
    private InputAction ultimateAbilityAction;

    //Инициализация статов персонажа
    public override void Initialize()
    {
        base.Initialize();

        CurrentMana = EntityStats.ModifiableStats[StatType.Mana].GetFinalValue();
        CurrentStamina = EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue();

        HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponentInChildren<BarScript>();
        ManaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponentInChildren<BarScript>();
        StaminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponentInChildren<BarScript>();

        //Animator
        animator.SetFloat("CharSpeedMult", AttackSpeed);

        //ICharacter
        EntityStats.SkillSet[SkillsType.Special].OnSkillTrigger += Character.SpecialAbility;
        EntityStats.SkillSet[SkillsType.Distract].OnSkillTrigger += Character.DistractionAbility;
        EntityStats.SkillSet[SkillsType.Ultimate].OnSkillTrigger += Character.UltimateAbility;

        EntityStats.SkillSet[SkillsType.Distract].ResetCooldown();
        EntityStats.SkillSet[SkillsType.Special].ResetCooldown();
        EntityStats.SkillSet[SkillsType.Ultimate].ResetCooldown();

        //Events
        OnDie += () => EntityStateMachine.CurrentState.InnerStateMachine.ChangeState(dyingState);

        OnTakeDamage += () =>
        {
            if (IsActive)
            {
                HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
            }
        };

        OnManaChange += () =>
        {
            ManaBar.ChangeBarValue(CurrentMana, EntityStats.ModifiableStats[StatType.Mana].GetFinalValue());
        };

        OnStaminaChange += () =>
        {
            if (IsActive)
            {
                StaminaBar.ChangeBarValue(CurrentStamina, EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue());
            }
        };

        InitializeInputEvents();
    }

    private void InitializeInputEvents()
    {
        PlayerInput playerInput = gameObject.GetComponent<PlayerInput>();
        //playerInput.actions = GameManager.GetInstance().playerInput;
        //playerInput.defaultActionMap = "Player";

        switchCharacterAction = playerInput.actions["SwitchCharacter"];
        distractAbilityAction = playerInput.actions["Distract"];
        specialAbilityAction = playerInput.actions["Special"];
        ultimateAbilityAction = playerInput.actions["Ultimate"];

        switchCharacterAction.performed += SwitchCharacter;

        distractAbilityAction.performed += ctx => TriggerSkillAnim(ctx.action.name);
        specialAbilityAction.performed += ctx => TriggerSkillAnim(ctx.action.name);
        ultimateAbilityAction.performed += ctx => TriggerSkillAnim(ctx.action.name);
    }

    public void TriggerSkillAnim(string name)
    {
        SkillsType result;
        Enum.TryParse(name, out result);
        Skill skill = EntityStats.SkillSet[result];
        if (CurrentMana >= skill.ManaCost & !skill.IsCooldown())
        {
            animator.SetTrigger(skill.SkillType.ToString());
        }
        else
        {
            Debug.Log($"Cannot activate skill!!! {skill.IsCooldown()}");
        }
    }

    #endregion [Character Script Stats]

    #region [Inventory]

    //TODO: Слоты инветаря для каждого персонажа
    public CharacterEquipment equipment;

    #endregion [Inventory]

    #region [IDamageable]
    public override void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanBlock)
    {
        int Damage = (int)Math.Floor(CombatManager.DamageCalc(DealerStats, EntityStats, Multiplier));

        if (!IsBlocking & !IsDodging)
        {
            CurrentHealth -= Damage;
            MiscUtilities.DamagePopUp(transform, Damage.ToString(), "#FFFFFF", 1);
            base.TakeDamage(DealerStats, Multiplier, CanBlock);
        }
        else if (CanBlock & blockingTime <= GameManager.GetInstance().ParryTime)
        {
            StartCoroutine(AddModifier(GameManager.GetInstance().ParryMod));
            EntityStateMachine.CurrentState.InnerStateMachine.ChangeState(new PlayerAttackState(this, EntityStateMachine.CurrentState.InnerStateMachine));
        }
        else
        {
            ChangeStamina(-(DealerStats.IsHeavy ? EntityStats.HeavyBlockCost : EntityStats.BlockCost));

            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                CurrentHealth -= Damage;
                Stun(4f);
            }
        }
    }

    public override void Stun(float Time)
    {
        base.Stun(Time);
        EntityStateMachine.CurrentState.InnerStateMachine.
        ChangeState(new StunnedState(this, EntityStateMachine.CurrentState.InnerStateMachine, Time));
    }

    #endregion [IDamageable]

    #region [State Machine]

    [Header("State Machine")]
    //Машина состояний
    [HideInInspector]
    public StateMachine EntityStateMachine;

    public string CurrentStateName, CurrentSubStateName;

    //Состояния
    [HideInInspector]
    public PlayerState playerState;

    [HideInInspector]
    public SidekickState sidekickState;

    private DyingState dyingState;

    public void InitializeStateMachine()
    {
        EntityStateMachine = new StateMachine();

        dyingState = new DyingState(gameObject, EntityStateMachine);
        playerState = new PlayerState(this, EntityStateMachine);
        sidekickState = new SidekickState(this, EntityStateMachine);

        EntityStateMachine.Initialize(IsActive ? playerState : sidekickState);
    }

    #endregion [State Machine]

    #region [Unity Methods]

    public override void Awake()
    {
        base.Awake();

        _camera = Camera.main;
    }

    public void Start()
    {
        InitializeStateMachine();

        //Текущие статы
        CurrentStamina = EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue();
        CurrentMana = EntityStats.ModifiableStats[StatType.Mana].GetFinalValue();

        equipment = new CharacterEquipment(this);
    }

    public virtual void Update()
    {
        EntityStateMachine.CurrentState?.LogicUpdate();

        CurrentStateName = EntityStateMachine.CurrentState?.GetType().Name;
        CurrentSubStateName = EntityStateMachine.CurrentState.InnerStateMachine.CurrentState?.GetType().Name;
    }

    public void LateUpdate()
    {
        EntityStats.SetLevel(this.Level);

        Speed = EntityStats.ModifiableStats[StatType.Speed].GetFinalValue();

        animator.SetFloat("CharSpeedMult", EntityStats.AttackSpeed);
    }

    #endregion [Unity Methods]

    #region [Utilities]

    public void Movement(Vector3 vector, float speed)
    {
        Vector3 MovementDirection = Quaternion.Euler(0, -45, 0) * vector;
        MovementDirection.Normalize();

        transform.Translate(MovementDirection * (speed * Time.deltaTime), Space.World);

        Quaternion toRotation = Quaternion.LookRotation(MovementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, RotationSpeed * Time.deltaTime);
    }

    public void StaminaRecovering()
    {
        float MaxStamina = EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue();

        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);

        if (CurrentStamina < MaxStamina)
            CurrentStamina += Time.deltaTime * 5f;

        CurrentStamina = CurrentStamina >= MaxStamina ? MaxStamina : CurrentStamina += Time.deltaTime * 5f;

        IsStaminaRecovering = CurrentStamina <= EntityStats.ModifiableStats[StatType.Stamina].GetProcent() * 30;

        OnStaminaChange();
    }

    public void ChangeStamina(float Amount)
    {
        CurrentStamina += Amount;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue());
        OnStaminaChange();
    }

    private void SwitchCharacter(InputAction.CallbackContext obj)
    {
        if (int.TryParse(obj.control.name, out int num) & num != 0)
            GameManager.GetInstance().partyManager.SwitchPlayer(this, num);
    }

    public void LookAtEnemy(float range)
    {
        List<EnemyScript> Enemies = GameObject.FindObjectsOfType<EnemyScript>().ToList();

        if (Enemies.Count != 0)
        {
            //Ближайший враг в радиусе
            GameObject NearestEnemy = AIUtilities.FindNearestEntity(gameObject.transform, AIUtilities.EnemyTag);

            if (AIUtilities.IsCertainEntityInRadius(gameObject, NearestEnemy, range))
                gameObject.transform.LookAt(NearestEnemy.transform.position);
        }
        else
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitData))
            {
                Vector3 lookAt = hitData.point;
                lookAt.y = 0;
                gameObject.transform.LookAt(lookAt);
            }
        }
    }

    public void DistractTrigger() => UseAbility(EntityStats.SkillSet[SkillsType.Distract]);

    public void SpecialTrigger() => UseAbility(EntityStats.SkillSet[SkillsType.Special]);

    public void UltimateTrigger() => UseAbility(EntityStats.SkillSet[SkillsType.Ultimate]);

    private void UseAbility(Skill skill)
    {
        if (!skill.IsCooldown() & CurrentMana >= skill.ManaCost)
        {
            float ManaCon = Mathf.Clamp(EntityStats.ModifiableStats[StatType.ManaConsumption].GetFinalValue(), 0, 80) / 100;
            GiveSupport(-skill.ManaCost * (1 - ManaCon), StatType.Mana);

            skill.ActivateSkill();
            LookAtEnemy(10f);

            StartCoroutine(MiscUtilities.Instance.ActionWithDelay(skill.CooldownInSecs, () => skill.ResetCooldown()));
        }
        else if (skill.IsCooldown())
            Debug.Log($"{skill.SkillName} is cooldown!!!");
        else
            Debug.Log($"Not enough mana! Need {skill.ManaCost}/{CurrentMana}");
    }

    public override void GiveSupport(float Amount, StatType type)
    {
        switch (type)
        {
            case StatType.Stamina:
                ChangeStamina(Amount);
                break;
            case StatType.Mana:
                var ManaRecBonus = Mathf.Clamp(EntityStats.ModifiableStats[StatType.ManaRecoveryBonus].GetFinalValue(), 0, 80) / 100;
                var AmountWithBouns = Amount * (1 + ManaRecBonus);
                CurrentMana = Mathf.Clamp(CurrentMana + AmountWithBouns, 0, EntityStats.ModifiableStats[type].GetFinalValue());
                break;
        }

        base.GiveSupport(Amount, type);
    }

    #endregion [Utilities]
}