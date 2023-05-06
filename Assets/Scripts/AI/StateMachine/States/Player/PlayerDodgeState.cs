using UnityEngine;

public class PlayerDodgeState : State
{
    Rigidbody rigidbody;

    float CooldownTimer;
    float Cooldown = 1f;

    public PlayerDodgeState(CharacterScript character, StateMachine stateMachine)
        : base(character, stateMachine)
    {
        this.rigidbody = character.gameObject.GetComponent<Rigidbody>();
    }

    public override void EnterState()
    {
        animator.SetTrigger("Dodge");
        CooldownTimer = 0;

        //rigidbody.isKinematic = false;
        //rigidbody.velocity = gameObject.transform.forward * 5f;

        animator.applyRootMotion = true;

        character.CurrentStamina -= character.EntityStats.DodgeCost;
        character.IsDodging = true;

        movementState = new PlayerMovementState(character, stateMachine);
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        if (CooldownTimer < Cooldown)
            CooldownTimer += Time.deltaTime;
        else
            stateMachine.ChangeState(movementState);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();
        character.IsDodging = false;
        rigidbody.isKinematic = true;

        animator.applyRootMotion = false;
    }
}
