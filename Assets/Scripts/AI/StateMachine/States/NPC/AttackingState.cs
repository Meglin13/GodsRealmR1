using UnityEngine;

public class AttackingState : State
{
    public AttackingState(GameObject gameObject,
    StateMachine stateMachine) : base(gameObject, stateMachine)
    {

    }

    float cooldown = 2f;

    //TODO: Добавить кулдауны

    public override void EnterState()
    {
        idleState = new IdleState(gameObject, stateMachine);
    }

    float timePassed = 0f;

    public override void LogicUpdate()
    {
        animator.SetFloat("Velocity", 0, 0.01f, Time.deltaTime);
        animator.SetTrigger("Attack");

        if (gameObject.TryGetComponent(out CharacterScript characterScript))
        {
            target = AIUtilities.FindNearestEntity(gameObject.transform, AIUtilities.EnemyTag);

            animator.SetTrigger("Move");
        }
        else
        {
            target = AIUtilities.FindNearestEntity(gameObject.transform, AIUtilities.CharsTag);
        }

        timePassed += Time.deltaTime;

        if (target != null)
            gameObject.transform.LookAt(target.transform.position);
    }
}
