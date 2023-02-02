using UnityEngine;

public class PlayerBlockState : State
{
    public PlayerBlockState(CharacterScript character, StateMachine stateMachine) : base(character, stateMachine)
    {
    }

    public override void EnterState()
    {
        animator.SetBool("IsBlocking", true);
        character.IsBlocking = true;
        character.blockingTime = 0;

        playerIdleState = new PlayerIdleState(character, stateMachine);
    }

    public override void LogicUpdate()
    {
        character.blockingTime += Time.deltaTime;

        if (!IsBlockAction)
        {
            stateMachine.ChangeState(playerIdleState);
        }
    }

    public override void ExitState()
    {
        base.ExitState();
        animator.SetBool("IsBlocking", false);
        character.IsBlocking = false;
    }
}
