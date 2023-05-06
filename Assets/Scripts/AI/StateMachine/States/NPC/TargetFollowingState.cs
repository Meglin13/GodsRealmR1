using UnityEngine;

public class TargetFollowingState : State
{
    public TargetFollowingState(GameObject target, GameObject actor, StateMachine stateMachine) : base(target, actor, stateMachine)
    {

    }

    private Vector3 position;

    public TargetFollowingState(Vector3 target, GameObject actor, StateMachine stateMachine) : base(target, actor, stateMachine)
    {
        position = target;
    }

    public override void EnterState()
    {
        agent.isStopped = false;
        animator.SetTrigger("Move");
    }

    public override void LogicUpdate()
    {
        Vector3 dir = target ? target.transform.position - gameObject.transform.position : position;
        dir.y = 0;
        float velocity = dir.magnitude / 10;


        float speed = 5;
        agent.speed = speed + velocity * 1.3f;

        animator.SetFloat("Velocity", agent.velocity.magnitude / agent.speed);

        agent.SetDestination(target.transform.position);
        Quaternion r = Quaternion.LookRotation(agent.velocity.normalized);
        if (r != Quaternion.identity & dir != Vector3.zero)
        {
            gameObject.transform.rotation = r;
        }

    }

    public override void ExitState()
    {
        agent.isStopped = true;
    }
}