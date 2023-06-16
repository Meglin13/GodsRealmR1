using UnityEngine;

public class PlayerMovementState : State
{
    Vector3 input;

    public PlayerMovementState(CharacterScript character, StateMachine stateMachine)
        : base(character, stateMachine)
    {

    }

    public override void EnterState()
    {
        dodgeState = new PlayerDodgeState(character, stateMachine);
        playerBlockState = new PlayerBlockState(character, stateMachine);

        animator.applyRootMotion = false;
        animator.SetTrigger("Move");

        playerIdleState = new PlayerIdleState(character, stateMachine);
    }

    public override void HandleInput()
    {
        base.HandleInput();

        Vector2 vector = movementAction.ReadValue<Vector2>();
        input = new(vector.x, 0f, vector.y);
    }

    public override void LogicUpdate()
    {
        character.velocity = input.magnitude;

        if (IsDodgeAction & character.CurrentStamina >= character.EntityStats.DodgeCost)
            stateMachine.ChangeState(dodgeState);

        if (blockAction.activeControl != null)
            stateMachine.ChangeState(playerBlockState);

        if (IsMovementAction)
        {
            float speed = IsSprintAction ? character.EntityStats.Sprint : character.EntityStats.ModifiableStats[StatType.Speed].GetFinalValue();

            if (IsSprintAction & !character.IsStaminaRecovering & character.CurrentStamina > 0)
            {
                character.ChangeStamina(-character.StaminaConsumption * Time.deltaTime);
            }
            else
            {
                character.velocity /= 2;
                character.StaminaRecovering();
            }

            animator.SetFloat("Velocity", character.velocity, character.acceleration, Time.deltaTime);
            character.Movement(input, character.velocity * speed);
        }
        else
        {
            stateMachine.ChangeState(playerIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
