using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// Класс состояния
/// </summary>
public abstract class State
{
    //Объекты взаимодействия
    public NavMeshAgent agent;
    public GameObject target;
    public GameObject gameObject;
    public Animator animator;

    public CharacterScript character;
    public EnemyScript enemy;

    public float AttackRange, SightRange;

    #region [Input Actions]
    //Ввод игрока
    public PlayerInput playerInput;

    public InputAction movementAction;
    public InputAction attackAction;
    public InputAction blockAction;
    public InputAction dodgeAction;
    public InputAction sprintAction;

    public bool IsMovementAction;
    public bool IsAttackAction;
    public bool IsBlockAction;
    public bool IsDodgeAction;
    public bool IsSprintAction;
    public bool IsSpecialAction;
    public bool IsUltimateAction;
    public bool IsDistractAction;
    public bool IsSwitchCharacterAction;
    #endregion

    #region [StateMachine]
    public StateMachine stateMachine;
    public StateMachine InnerStateMachine;

    public AttackingState attackingState;
    public IdleState idleState;

    public PlayerIdleState playerIdleState;
    public PlayerMovementState movementState;
    public PlayerDodgeState dodgeState;
    public PlayerAttackState playerAttackState;
    public PlayerBlockState playerBlockState;

    public TargetFollowingState playerFollowing;
    public DyingState dyingState;

    public PlayerState playerState;
    public SidekickState sidekickState;
    #endregion

    /// <summary>
    /// Конструктор для взаимодействия с заданной целью
    /// </summary>
    /// <param _name="target">Цель</param>
    /// <param _name="actor">Исполнитель</param>
    /// <param _name="stateMachine">Машина состояний</param>
    public State(GameObject target, GameObject actor, StateMachine stateMachine)
    {
        this.target = target;
        this.agent = actor.GetComponent<NavMeshAgent>();
        this.animator = actor.GetComponent<Animator>();
        this.stateMachine = stateMachine;
        this.gameObject = actor;
    }

    public State(Vector3 target, GameObject actor, StateMachine stateMachine) : this(null, actor, stateMachine)
    {

    }

    /// <summary>
    /// Конструктор для врагов
    /// </summary>
    /// <param _name="enemy">Враг</param>
    /// <param _name="stateMachine">Машина состояний</param>
    public State(EnemyScript enemy, StateMachine stateMachine)
    {
        this.agent = enemy.GetComponent<NavMeshAgent>();
        this.animator = enemy.GetComponent<Animator>();
        this.stateMachine = stateMachine;
        this.gameObject = enemy.gameObject;
    }

    /// <summary>
    /// Общий конструктор
    /// </summary>
    /// <param _name="gameObject"></param>
    /// <param _name="stateMachine"></param>
    public State(GameObject gameObject, StateMachine stateMachine)
    {
        this.agent = gameObject.GetComponent<NavMeshAgent>();
        this.animator = gameObject.GetComponent<Animator>();
        this.stateMachine = stateMachine;
        this.gameObject = gameObject;
    }

    /// <summary>
    /// Конструктор для персонажей
    /// </summary>
    /// <param _name="character"></param>
    /// <param _name="stateMachine"></param>
    public State(CharacterScript character, StateMachine stateMachine)
    {
        this.gameObject = character.gameObject;
        this.stateMachine = stateMachine;
        this.character = character;

        this.animator = gameObject.GetComponent<Animator>();
        this.agent = gameObject.GetComponent<NavMeshAgent>();

        playerInput = character.GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Movement"];
        attackAction = playerInput.actions["Attack"];
        blockAction = playerInput.actions["Block"];
        dodgeAction = playerInput.actions["Dodge"];
        sprintAction = playerInput.actions["Sprint"];
    }

    //Вход в состояние
    public abstract void EnterState();

    //Метод ввода для игрока
    public virtual void HandleInput()
    {
        if (playerInput)
        {
            IsDodgeAction = dodgeAction.triggered;
            IsMovementAction = !(movementAction.triggered || movementAction.ReadValue<Vector2>().sqrMagnitude == 0f);
            IsSprintAction = sprintAction.activeControl != null & IsMovementAction;

            IsAttackAction = attackAction.triggered;
            IsBlockAction = blockAction.activeControl != null;
        }
    }

    //Логика
    public abstract void LogicUpdate();

    //Физика
    public virtual void PhysicsUpdate() { }

    //Выход из состояния
    public virtual void ExitState() { }

    internal void CancelAnimation()
    {
        if (animator)
        {
            animator.enabled = false;
            animator.enabled = true;
        }
    }
}