using MyBox;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InteractorScript))]
[RequireComponent(typeof(Animator))]
[SelectionBase]
public abstract class CharacterScript : EntityScript
{
#if UNITY_EDITOR
    [ButtonMethod]
    public override void OnValidate()
    {
        base.OnValidate();

        if (EntityStats)
        {
            GetComponent<PlayerInput>().actions = Resources.Load<InputActionAsset>("InputSystem/PlayerActions");
        }
    }
#endif

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

    public CharacterEquipment equipment;

    //Интерфейс персонажа
    internal ICharacter Character;
    internal Skill Special;
    internal Skill Distract;
    internal Skill Ultimate;

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

    [HideInInspector]
    public PlayerInput playerInput;

    private InputAction switchCharacterAction;
    private InputAction specialAbilityAction;
    private InputAction distractAbilityAction;
    private InputAction ultimateAbilityAction;
    //private InputAction commanderAction;
    //private InputAction leftAction;
    //private InputAction rightAction;

    [HideInInspector]
    public InteractorScript interactor;

    //Инициализация статов персонажа
    public override void Initialize()
    {
        base.Initialize();

        _camera = Camera.main;

        CurrentMana = EntityStats.ModifiableStats[StatType.Mana].GetFinalValue();
        CurrentStamina = EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue();

        HealthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponentInChildren<BarScript>();
        ManaBar = GameObject.FindGameObjectWithTag("ManaBar").GetComponentInChildren<BarScript>();
        StaminaBar = GameObject.FindGameObjectWithTag("StaminaBar").GetComponentInChildren<BarScript>();

        //Interactor
        interactor = GetComponent<InteractorScript>();

        //Animator
        animator = GetComponent<Animator>();

        //ICharacter
        Special = EntityStats.SkillSet[SkillType.Special];
        Distract = EntityStats.SkillSet[SkillType.Distract];
        Ultimate = EntityStats.SkillSet[SkillType.Ultimate];

        Special.OnSkillTrigger += Character.SpecialAbility;
        Distract.OnSkillTrigger += Character.DistractionAbility;
        Ultimate.OnSkillTrigger += Character.UltimateAbility;

        Special.ResetCooldown();
        Distract.ResetCooldown();
        Ultimate.ResetCooldown();

        //Events
        OnDie += () => EntityStateMachine.CurrentState.InnerStateMachine.ChangeState(dyingState);

        OnTakeDamage += UpdateBars;
        OnManaChange += UpdateBars;
        OnStaminaChange += UpdateBars;
        OnHeal += UpdateBars;

        OnAddModifier += UpdateBars;
        OnAddModifier += UpdateSpeed;

        InitializeInputEvents();
    }

    private void InitializeInputEvents()
    {
        playerInput = gameObject.GetComponent<PlayerInput>();

        switchCharacterAction = playerInput.actions["SwitchCharacter"];
        distractAbilityAction = playerInput.actions["Distract"];
        specialAbilityAction = playerInput.actions["Special"];
        ultimateAbilityAction = playerInput.actions["Ultimate"];
        //commanderAction = playerInput.actions["Commander"];
        //leftAction = playerInput.actions["Attack"];
        //rightAction = playerInput.actions["Block"];

        switchCharacterAction.performed += SwitchCharacter;
        distractAbilityAction.performed += TriggerSkillAnim;
        specialAbilityAction.performed += TriggerSkillAnim;
        ultimateAbilityAction.performed += TriggerSkillAnim;

    }

    #endregion [Character Script Stats]

    #region [IDamageable]
    public override void TakeDamage(EntityStats DealerStats, float Multiplier, bool CanBlock)
    {
        int Damage = Mathf.FloorToInt(CombatManager.DamageCalc(EntityStats, DealerStats, Multiplier));

        if (!IsBlocking & !IsDodging)
        {
            CurrentHealth -= Damage;
            MiscUtilities.DamagePopUp(transform, Damage.ToString(), "#FFFFFF", 1);
            base.TakeDamage(DealerStats, Multiplier, CanBlock);

            if (IsActive)
                impulseSource.GenerateImpulse(0.05f);
        }
        else if (CanBlock & blockingTime <= GameManager.Instance.ParryTime)
        {
            GameManager.Instance.StartCoroutine(AddModifier(GameManager.Instance.ParryMod));
            
            EntityStateMachine.CurrentState.InnerStateMachine.ChangeState(new PlayerAttackState(this, EntityStateMachine.CurrentState.InnerStateMachine));
        }
        else
        {
            ChangeStamina(-(DealerStats.IsHeavy ? EntityStats.HeavyBlockCost : EntityStats.BlockCost));

            if (CurrentStamina <= 0)
            {
                CurrentStamina = 0;
                CurrentHealth -= Damage;
                Stun(3f);
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

#if UNITY_EDITOR
    public string CurrentStateName, CurrentSubStateName;
#endif

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
        Initialize();

        equipment = new CharacterEquipment(this);
    }

    public void OnEnable()
    {
        UpdateSpeed();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();

        ObjectsUtilities.UnsubscribeEvents(OnStaminaChange);

        Special.ClearEvents();
        Distract.ClearEvents();
        Ultimate.ClearEvents();

        ultimateAbilityAction.performed -= TriggerSkillAnim;
        specialAbilityAction.performed -= TriggerSkillAnim;
        distractAbilityAction.performed -= TriggerSkillAnim;
        switchCharacterAction.performed -= SwitchCharacter;

        Special.OnSkillTrigger -= Character.SpecialAbility;
        Distract.OnSkillTrigger -= Character.DistractionAbility;
        Ultimate.OnSkillTrigger -= Character.UltimateAbility;
    }

    public void Start()
    {
        InitializeStateMachine();

        //Текущие статы
        CurrentStamina = EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue();
        CurrentMana = EntityStats.ModifiableStats[StatType.Mana].GetFinalValue();
        CurrentHealth = EntityStats.ModifiableStats[StatType.Health].GetFinalValue();

        animator.SetFloat("CharSpeedMult", EntityStats.AttackSpeed);
    }

    public virtual void Update()
    {
        EntityStateMachine.CurrentState?.LogicUpdate();

#if UNITY_EDITOR
        CurrentStateName = EntityStateMachine.CurrentState?.GetType().Name;
        CurrentSubStateName = EntityStateMachine.CurrentState.InnerStateMachine.CurrentState?.GetType().Name;
#endif
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

        if (CurrentStamina < MaxStamina)
            CurrentStamina += Time.deltaTime * 5f;

        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
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
        {
            //if (commanderAction.activeControl != null | commanderAction.triggered)
            //{
            //    if (leftAction.triggered || leftAction.activeControl != null)
            //    {
            //        GameManager.Instance.partyManager.GiveCommandToMember(CommandType.GoToPoint, num);
            //    }
            //    else if (rightAction.triggered || rightAction.activeControl != null)
            //    {
            //        GameManager.Instance.partyManager.GiveCommandToMember(CommandType.ComeToLeader, num);
            //    }
            //}
            //else
            {
                GameManager.Instance.partyManager.SwitchPlayer(this, num - 1);
            }
        }
    }

    public void LookAtEnemy(float range)
    {
        Collider[] Enemies = Physics.OverlapSphere(transform.position, range, 1 << EnemyLayer);

        if (Enemies.Length > 0)
        {
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

                Debug.DrawLine(ray.origin, ray.origin + ray.direction * 250, Color.red);
            }
        }
    }

    #region [Animation Events]
    public void DistractTrigger() => UseAbility(Distract);
    public void SpecialTrigger() => UseAbility(Special);
    public void UltimateTrigger() => UseAbility(Ultimate);
    #endregion

    public void TriggerSkillAnim(InputAction.CallbackContext ctx)
    {
        string name = ctx.action.name;

        Enum.TryParse(name, out SkillType result);
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

    private void UseAbility(Skill skill)
    {
        if (!skill.IsCooldown() & CurrentMana >= skill.ManaCost)
        {
            float ManaCon = Mathf.Clamp(EntityStats.ModifiableStats[StatType.ManaConsumption].GetFinalValue(), 0, 80) / 100;
            GiveSupport(-skill.ManaCost * (1 - ManaCon), StatType.Mana);

            skill.ActivateSkill();
            LookAtEnemy(EntityStats.AttackRange);

            GameManager.Instance.StartCoroutine(MiscUtilities.Instance.ActionWithDelay(skill.CooldownInSecs, () => skill.ResetCooldown()));
        }
        else if (skill.IsCooldown())
            Debug.Log($"{skill.Name} is cooldown!!!");
        else
            Debug.Log($"Not enough mana! Need {skill.ManaCost}/{CurrentMana}");
    }

    public void UpdateBars()
    {
        if (IsActive)
        {
            HealthBar.ChangeBarValue(CurrentHealth, EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
            ManaBar.ChangeBarValue(CurrentMana, EntityStats.ModifiableStats[StatType.Mana].GetFinalValue());
            StaminaBar.ChangeBarValue(CurrentStamina, EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue());
        }
    }

    private void UpdateSpeed()
    {
        animator.SetFloat("CharSpeedMult", EntityStats.AttackSpeed);
        Speed = EntityStats.ModifiableStats[StatType.Speed].GetFinalValue();
    }

    public override void GiveSupport(float Amount, StatType type)
    {
        base.GiveSupport(Amount, type);

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
    }

    #endregion [Utilities]
}