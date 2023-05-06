public class PlayerState : State
{
    public PlayerState(CharacterScript character,
    StateMachine stateMachine) : base(character, stateMachine)
    {

    }

    public override void EnterState()
    {
        CancelAnimation();

        playerInput.enabled = true;
        character.interactor.enabled = true;

        InnerStateMachine = new StateMachine();

        var playerIdleState = new PlayerIdleState(character, InnerStateMachine);

        InnerStateMachine.Initialize(playerIdleState);

        character.HealthBar.ChangeBarValue(character.CurrentHealth, character.EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
        character.ManaBar.ChangeBarValue(character.CurrentMana, character.EntityStats.ModifiableStats[StatType.Mana].GetFinalValue());
        character.StaminaBar.ChangeBarValue(character.CurrentStamina, character.EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue());
    }

    public override void HandleInput()
    {
        InnerStateMachine.CurrentState.HandleInput();
    }

    public override void LogicUpdate()
    {
        HandleInput();

        InnerStateMachine.CurrentState.LogicUpdate();
    }


    public override void PhysicsUpdate()
    {
        InnerStateMachine.CurrentState.PhysicsUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();
        character.IsActive = false;
        playerInput.enabled = false;
        InnerStateMachine.CurrentState.ExitState();
    }
}