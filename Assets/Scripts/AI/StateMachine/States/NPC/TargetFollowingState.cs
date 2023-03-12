using UnityEngine;

public class TargetFollowingState : State
{
    public TargetFollowingState(GameObject target, GameObject actor, StateMachine stateMachine) : base(target, actor, stateMachine)
    {

    }

    public override void EnterState()
    {
        agent.isStopped = false;
        animator.SetTrigger("Move");
    }

    public override void LogicUpdate()
    {
        Vector3 dir = target.transform.position - gameObject.transform.position;
        dir.y = 0;

        float velocity = dir.magnitude / 10;

        float speed = 5;
        agent.speed = speed + velocity * 1.3f;

        animator.SetFloat("Velocity", agent.velocity.magnitude / agent.speed);

        agent.SetDestination(target.transform.position);
        Quaternion r = Quaternion.LookRotation(agent.velocity.normalized);
        if (r != Quaternion.identity)
        {
            gameObject.transform.rotation = r;
        }
        
    }

    public override void ExitState()
    {
        agent.isStopped = true;
    }
}
