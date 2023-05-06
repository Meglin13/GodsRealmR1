using UnityEngine;

public class StunnedState : State
{
    private float StunTime;
    private float passedTime = 0f;
    private State ReturnState;

    public StunnedState(GameObject gameObject, StateMachine stateMachine, float StunTime) : base(gameObject, stateMachine)
    {
        this.StunTime = StunTime;

        ReturnState = new IdleState(gameObject, stateMachine);
    }

    public StunnedState(CharacterScript character, StateMachine stateMachine, float StunTime) : base(character, stateMachine)
    {
        this.StunTime = StunTime;

        if (character.IsActive)
        {
            ReturnState = new PlayerIdleState(character, stateMachine);
        }
    }

    public override void EnterState()
    {
        animator.SetBool("IsStunned", true);
        passedTime = 0f;

    }

    public override void LogicUpdate()
    {
        if (passedTime >= StunTime)
        {
            stateMachine.ChangeState(ReturnState);
        }
        passedTime += Time.deltaTime;
    }

    public override void ExitState()
    {
        animator.SetBool("IsStunned", false);
        base.ExitState();
    }
}