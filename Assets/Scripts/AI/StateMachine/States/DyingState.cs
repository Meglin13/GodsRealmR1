using UnityEngine;

public class DyingState : State
{
    public DyingState(GameObject gameObject, StateMachine stateMachine) : base(gameObject, stateMachine)
    {

    }

    public override void EnterState()
    {
        if (gameObject.TryGetComponent(out CharacterScript character))
        {
            if (character.IsActive)
            {
                SidekickState sidekickState = new(character, character.EntityStateMachine);
                character.EntityStateMachine.ChangeState(sidekickState);

                CameraCenterBehaviour.Instance.gameObject.transform.SetParent(null);
                PartyManager.Instance.CharacterDeath(character);
            }
        }

        animator.SetTrigger("Death");

        GameManager.Instance.StartCoroutine(MiscUtilities.Instance.ActionWithDelay(0.5f, () => gameObject.SetActive(false)));
    }

    public override void LogicUpdate() { }
}