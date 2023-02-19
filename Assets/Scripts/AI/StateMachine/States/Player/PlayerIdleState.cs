using UnityEngine;

public class PlayerIdleState : State
{
    float IdleTime;


    public PlayerIdleState(CharacterScript character, StateMachine stateMachine)
        : base(character, stateMachine)
    {

    }

    public override void EnterState()
    {
        IdleTime = 10f;

        playerBlockState = new PlayerBlockState(character, stateMachine);
        movementState = new PlayerMovementState(character, stateMachine);
        playerAttackState = new PlayerAttackState(character, stateMachine);
        playerIdleState = new PlayerIdleState(character, stateMachine);

        animator.applyRootMotion = false;

        animator.SetFloat("Velocity", 0f, -character.acceleration, Time.deltaTime);
    }

    public override void LogicUpdate()
    {
        if (!(IsSprintAction & !character.IsStaminaRecovering))
            character.StaminaRecovering();

        if (IsMovementAction)
        {
            stateMachine.ChangeState(movementState);
        }

        if (IsAttackAction)
        {
            stateMachine.ChangeState(playerAttackState);
        }

        if (IsBlockAction)
        {
            stateMachine.ChangeState(playerBlockState);
        }

        //TODO: Анимация ожидания
        //if (IdleTime > 0)
        //    IdleTime -= Time.deltaTime;
        //else
        //{
        //    animator.SetTrigger("Idle");
        //    IdleTime = 4f + animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        //}
    }

    public override void ExitState()
    {
        animator.SetTrigger("Move");
    }
}