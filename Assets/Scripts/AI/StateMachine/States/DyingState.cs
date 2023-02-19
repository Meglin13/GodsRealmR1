using UnityEngine;

public class DyingState : State
{
    public DyingState(GameObject gameObject, StateMachine stateMachine) : base(gameObject, stateMachine)
    {

    }

    public override void EnterState()
    {
        if (gameObject.TryGetComponent<CharacterScript>(out CharacterScript character))
        {
            if (character.IsActive)
            {
                SidekickState sidekickState = new(character, character.EntityStateMachine);
                character.EntityStateMachine.ChangeState(sidekickState);

                GameObject.FindObjectOfType<CameraCenterBehaviour>().gameObject.transform.SetParent(null);

                PartyManager partyManager = FindObjectOfType<PartyManager>();
                partyManager.CharDeath(character);
            }
        }

        animator.SetTrigger("Death");

        Destroy(gameObject, 1.5f);
    }

    public override void LogicUpdate()
    {

    }
}