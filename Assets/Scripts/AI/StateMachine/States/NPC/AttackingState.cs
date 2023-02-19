using UnityEngine;

public class AttackingState : State
{
    IDamageable damageable;

    public AttackingState(IDamageable damageable,
    StateMachine stateMachine) : base(damageable.gameObject, stateMachine)
    {
        this.damageable = damageable;
    }

    public override void EnterState()
    {
        idleState = new IdleState(gameObject, stateMachine);
    }

    float timePassed = 0f;

    public override void LogicUpdate()
    {
        animator.SetFloat("Velocity", 0, 0.01f, Time.deltaTime);
        animator.SetTrigger("Attack");

        target = AIUtilities.FindNearestEntity(gameObject.transform, damageable.EnemyTag);

        timePassed += Time.deltaTime;

        if (target)
            gameObject.transform.LookAt(target.transform.position);
    }
}
