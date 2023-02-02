using UnityEngine;

public class PlayerAttackState : State
{
    float timePassed;
    float currentClipLength;

    public PlayerAttackState(CharacterScript character, StateMachine stateMachine)
        : base(character, stateMachine)
    {

    }

    public override void EnterState()
    {
        animator.SetTrigger("Attack");

        animator.applyRootMotion = true;
        timePassed = 0f;

        currentClipLength = 1f;

        IsAttackAction = false;

        playerAttackState = new PlayerAttackState(character, stateMachine);
        playerIdleState = new PlayerIdleState(character, stateMachine);

        character.LookAtEnemy();
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void LogicUpdate()
    {
        timePassed += Time.deltaTime;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //if (currentClipLength <= 0)
        //{
        //    currentClipLength = stateInfo.length / stateInfo.speed;
        //}

        if (currentClipLength > 1.5f)
        {
            Debug.Log(currentClipLength + "/" + timePassed);
        }

        if (timePassed >= currentClipLength/2 & IsAttackAction)
        {
            stateMachine.ChangeState(playerAttackState);
        }

        if (timePassed >= currentClipLength)
        {
            stateMachine.ChangeState(playerIdleState);
            animator.SetTrigger("Move");
            CancelAnimation();
        }
    }

    public override void ExitState()
    {
        base.ExitState();
    }
}