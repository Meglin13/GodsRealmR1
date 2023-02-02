using UnityEngine;

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

        InnerStateMachine = new StateMachine();

        var playerIdleState = new PlayerIdleState(character, InnerStateMachine);

        InnerStateMachine.Initialize(playerIdleState);
    }

    public override void HandleInput()
    {
        InnerStateMachine.CurrentState.HandleInput();
    }

    public override void LogicUpdate()
    {
        HandleInput();

        InnerStateMachine.CurrentState.LogicUpdate();

        UpdateBars();
    }

    private void UpdateBars()
    {
        character.HPSB.ChangeBarValue(character.CurrentHealth, character.EntityStats.ModifiableStats[StatType.Health].GetFinalValue());
        character.MSB.ChangeBarValue(character.CurrentMana, character.EntityStats.ModifiableStats[StatType.Mana].GetFinalValue());
        character.SSB.ChangeBarValue(character.CurrentStamina, character.EntityStats.ModifiableStats[StatType.Stamina].GetFinalValue());
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