using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(PlayerInput))]

[SelectionBase]
public class CharacterScript : MonoBehaviour, IDamageable
{
    #region [Party Setup Settings]
    [Header("Party Setup Settings")]
    public int HotKeyNumber;
    public bool IsActive;
    #endregion

    #region [Character Script Stats]
    //Статы персонажа
    [Header("Character Stats")]
    [Range(1, 100)]
    public int Level = 1;
    [SerializeField]
    [MustBeAssigned]
    private EntityStats CharStats;
    public float blockingTime;
    Camera _camera;

    //Интерфейс персонажа
    internal ICharacter Character;

    //Текущее состояние атрибутов
    [Header("Current Stats")]
    public float CurrentStamina;
    public float CurrentMana;
    public bool IsBlocking, IsDodging;
    private Stat WeaponLengthStat;

    //Скорость и стамина
    [Header("BaseSpeed and Stamina")]
    [HideInInspector]
    public float velocity = 0f;
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
    public BarScript HPSB, MSB, SSB;
    internal MeleeWeapon Weapon;

    private InputAction switchCharacterAction;
    private InputAction specialAbilityAction;
    private InputAction distractAbilityAction;
    private InputAction ultimateAbilityAction;

    private Animator animator;
    private PlayerInput playerInput;

    public GameObject NormalAttackBullet;

    //Инициализация статов персонажа
    private void InitializePlayerStats()
    {
        gameObject.tag = "Character";
        gameObject.layer = AIUtilities.CharsLayer;

        //Объекты Unity
        if (NormalAttackBullet)
        {
            BulletsScript bulletsScript = NormalAttackBullet.GetComponent<BulletsScript>();
            bulletsScript.TargetLayer = AIUtilities.EnemyLayer;
        }

        Weapon = GetComponentInChildren<MeleeWeapon>();

        HPSB = GameObject.FindGameObjectWithTag("HealthBar").GetComponentInChildren<BarScript>();
        MSB = GameObject.FindGameObjectWithTag("ManaBar").GetComponentInChildren<BarScript>();
        SSB = GameObject.FindGameObjectWithTag("StaminaBar").GetComponentInChildren<BarScript>();

        //Инициализация объекта статов
        EntityStats.Init(Level);

        //PlayerInput
        playerInput = gameObject.GetComponent<PlayerInput>();

        playerInput.actions = GameManager.Instance.playerInput;
        playerInput.defaultActionMap = "Player";

        //Collider
        CapsuleCollider collider = gameObject.GetComponent<CapsuleCollider>();
        collider.height = 2;
        collider.center = new Vector3(0, 1, 0);

        //RigidBody
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;

        //Animator
        animator = GetComponent<Animator>();
        animator.SetFloat("CharSpeedMult", AttackSpeed);

        //ICharacter
        CharStats.SkillSet[SkillsType.Special].SkillAction = () => Character.SpecialAbility();
        CharStats.SkillSet[SkillsType.Distract].SkillAction = () => Character.DistractionAbility();
        CharStats.SkillSet[SkillsType.Ultimate].SkillAction = () => Character.UltimateAbility();
    }

    private void InitializeInputEvents()
    {
        switchCharacterAction =  playerInput.actions["SwitchCharacter"];
        distractAbilityAction = playerInput.actions["Distract"];
        specialAbilityAction = playerInput.actions["Special"];
        ultimateAbilityAction = playerInput.actions["Ultimate"];

        switchCharacterAction.performed += SwitchCharacter;

        distractAbilityAction.performed += ctx => animator.SetTrigger("Distract");
        specialAbilityAction.performed += ctx => animator.SetTrigger("Special");
        ultimateAbilityAction.performed += ctx => animator.SetTrigger("Ultimate");
    }
    #endregion

    #region [Inventory]
    //TODO: Слоты инветаря для каждого персонажа
    private Dictionary<EquipmentType, Item> EquipmentSlots = new Dictionary<EquipmentType, Item>()
    {
        { EquipmentType.Helmet, null }
    };
    #endregion

    #region [IDamageable]
    //Здоровье
    public float MaxHealth { get; set; }
    public float CurrentHealth { get; set; }
    public EntityStats EntityStats { get; set; }
    public void TakeDamage(EntityStats enemyStats, float Multiplier, bool CanParry)
    {
        int Damage = (int)Math.Floor(CombatManager.DamageCalc(enemyStats, CharStats, Multiplier));

        if (!IsBlocking & !IsDodging)
        {
            CurrentHealth -= Damage;

            MiscUtilities.DamagePopUp(transform, Damage.ToString(), "#FFFFFF", 1);

            if (CurrentHealth <= 0)
                Death();
        }
        else if (CanParry & blockingTime < GameManager.Instance.ParryTime)
        {
            StartCoroutine(AddModifier(GameManager.Instance.ParryMod));
            CharacterStateMachine.CurrentState.InnerStateMachine.ChangeState(new PlayerAttackState(this, CharacterStateMachine.CurrentState.InnerStateMachine));
        }
        else
        {
            CurrentStamina -= enemyStats.IsHeavy ? enemyStats.HeavyBlockCost : enemyStats.BlockCost;

            if (CurrentStamina < 0)
            {
                CurrentStamina = 0;
                CurrentHealth -= Damage;
                Stun(2f);
            }
        }
    }

    public void Stun(float Time)
    {
        StunnedState StunnedState = new StunnedState(this, CharacterStateMachine.CurrentState.InnerStateMachine, Time);
        CharacterStateMachine.CurrentState.InnerStateMachine.ChangeState(StunnedState);
    }

    public virtual void Death()
    {
        CharacterStateMachine.CurrentState.InnerStateMachine.ChangeState(dyingState);
    }

    public void HitVFX(Vector3 hitPosition)
    {
        var hitVFX = GameManager.Instance.HitVFX;
        GameObject hit = Instantiate(hitVFX, hitPosition, Quaternion.identity);
        Destroy(hit, 0.5f);
    }
    #endregion

    #region [State Machine]
    [Header("State Machine")]
    //Машина состояний
    [HideInInspector]
    public StateMachine CharacterStateMachine;
    public string CurrentStateName, CurrentSubStateName;

    //Состояния
    [HideInInspector]
    public PlayerState playerState;
    [HideInInspector]
    public SidekickState sidekickState;
    private DyingState dyingState;

    public void InitializeStateMachine()
    {
        CharacterStateMachine = new StateMachine();

        dyingState = new DyingState(gameObject, CharacterStateMachine);
        playerState = new PlayerState(this, CharacterStateMachine);
        sidekickState = new SidekickState(this, CharacterStateMachine);

        if (IsActive)
        {
            Debug.Log("Player");

            CharacterStateMachine.Initialize(playerState);
        }
        else
        {
            Debug.Log("Side");

            CharacterStateMachine.Initialize(sidekickState);
        }

    }
    #endregion

    #region [Unity Methods]
    public virtual void Awake()
    {
        EntityStats = CharStats;

        InitializePlayerStats();

        _camera = Camera.main;
    }

    public void Start()
    {
        InitializeStateMachine();

        if (Weapon)
            EntityStats.WeaponLengthStat = Weapon.WeaponLength;

        InitializeInputEvents();

        //Текущие статы
        CurrentStamina = CharStats.ModifiableStats[StatType.Stamina].GetFinalValue();
        CurrentHealth = CharStats.ModifiableStats[StatType.Health].GetFinalValue();
        CurrentMana = CharStats.ModifiableStats[StatType.Mana].GetFinalValue();
    }

    public virtual void Update()
    {
        CharacterStateMachine.CurrentState.LogicUpdate();

        CurrentStateName = CharacterStateMachine.CurrentState.GetType().Name;
        CurrentSubStateName = CharacterStateMachine.CurrentState.InnerStateMachine.CurrentState.GetType().Name;
    }

    public void LateUpdate()
    {
        //CharStats.SetLevel(Level);

        CharStats.UpdateStats();
        Speed = EntityStats.ModifiableStats[StatType.Speed].GetFinalValue();

        animator.SetFloat("CharSpeedMult", CharStats.AttackSpeed);
    }

    //private void FixedUpdate()
    //{
    //    CharacterStateMachine.CurrentState.PhysicsUpdate();
    //}
    #endregion

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
        float MaxStamina = CharStats.ModifiableStats[StatType.Stamina].GetFinalValue();

        if (CurrentStamina >= MaxStamina)
            CurrentStamina = MaxStamina;
        else if (CurrentStamina < 0)
            CurrentStamina = 0;
        else
            CurrentStamina += Time.deltaTime * 5f;

        CurrentStamina = CurrentStamina >= MaxStamina ? MaxStamina : CurrentStamina += Time.deltaTime * 5f;

        IsStaminaRecovering = CurrentStamina <= CharStats.ModifiableStats[StatType.Stamina].GetProcent() * 30;
    }

    private void SwitchCharacter(InputAction.CallbackContext obj)
    {
        PartyManager partyManager = GameObject.FindObjectOfType<PartyManager>();

        //TODO: Переключение на геймпаде

        int.TryParse(obj.control.name, out int num);

        if (num != 0)
            partyManager.SwitchPlayer(this, num);
    }

    public void LookAtEnemy()
    {
        List<EnemyScript> Enemies = GameObject.FindObjectsOfType<EnemyScript>().ToList();

        if (Enemies.Count != 0)
        {
            //Ближайший враг в радиусе
            GameObject NearestEnemy = AIUtilities.FindNearestEntity(gameObject.transform, AIUtilities.EnemyTag);

            if (AIUtilities.IsCertainEntityInRadius(gameObject, NearestEnemy, CharStats.AttackRange))
                gameObject.transform.LookAt(NearestEnemy.transform.position);
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
    }

    public void StartDealMeleeDamage()
    {
        Weapon.StartDealDamage();
        Weapon.Init(EntityStats, 70, false);
    }

    public void EndDealMeleeDamage() => Weapon.EndDealDamage();

    //TODO: Bruh
    public void DistractTrigger()
    {
        UseAbility(CharStats.SkillSet[SkillsType.Distract]);
    }

    public void SpecialTrigger()
    {
        UseAbility(CharStats.SkillSet[SkillsType.Special]);
    }

    public void UltimateTrigger()
    {
        UseAbility(CharStats.SkillSet[SkillsType.Ultimate]);
    }

    private void UseAbility(Skill skill)
    {
        if (!skill.IsCooldown() & CurrentMana >= skill.ManaCost)
        {
            CurrentMana -= skill.ManaCost;

            skill.SkillAction();
            skill.OnSkillTriggered();

            skill.SetCooldown();
            StartCoroutine(MiscUtilities.Instance.Cooldown(skill.CooldownInSecs, () => skill.ResetCooldown()));
        }
        else if (skill.IsCooldown())
        {
            Debug.Log($"{skill.SkillName} is cooldown!!!");
        }
        else
        {
            Debug.Log($"Not enough mana! Need {skill.ManaCost}/{CurrentMana}");
        }
    }

    public IEnumerator AddModifier(Modifier modifier)
    {
        if (modifier != null)
        {
            //Debug.Log($"Added modifier {modifier.Amount} for {modifier.DurationInSecs} sec. Stat before modifier {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");

            CharStats.ModifiableStats[modifier.StatType].AddModifier(modifier);

            //Debug.Log($"Stat after modifier {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");

            yield return new WaitForSeconds(modifier.DurationInSecs);

            if (!modifier.IsPermanent)
            {
                if (CharStats.ModifiableStats[modifier.StatType].ContainsModifier(modifier))
                {
                    CharStats.ModifiableStats[modifier.StatType].RemoveModifier(modifier);
                    //Debug.Log($"Removed modifier. Current stat value {CharStats.ModifiableStats[modifier.StatType].GetFinalValue()}");
                }
            }
        }
        else
            throw new Exception("Modifier is NULL");
    }
    #endregion
}