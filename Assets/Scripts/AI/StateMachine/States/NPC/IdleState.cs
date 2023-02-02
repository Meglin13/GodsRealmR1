using UnityEngine;

public class IdleState : State
{
    public IdleState(GameObject gameObject, StateMachine stateMachine) : base(gameObject, stateMachine)
    {

    }

    public override void EnterState()
    {
        agent.velocity = Vector3.zero;
        agent.Stop();
    }

    public override void LogicUpdate()
    {
        Vector3 input = new(0, 0f, 0);

        float velocity = input.magnitude;

        animator.SetFloat("Velocity", velocity, 0.05f, Time.deltaTime);
    }
}