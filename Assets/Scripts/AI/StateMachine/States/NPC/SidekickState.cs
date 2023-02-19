using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SidekickState : State
{

    private bool IsEnemyNearby;
    private bool IsPlayerNearby;

    GameObject NearestEnemy;
    GameObject Player;

    TargetFollowingState enemyFollowing;

    public SidekickState(CharacterScript character,
    StateMachine stateMachine) : base(character, stateMachine)
    {
        InnerStateMachine = new StateMachine();
    }

    public override void EnterState()
    {
        playerInput.enabled = false;

        InnerStateMachine = new StateMachine();

        idleState = new IdleState(gameObject, InnerStateMachine);
        attackingState = new AttackingState(character, InnerStateMachine);

        InnerStateMachine.Initialize(idleState);

        AttackRange = character.EntityStats.AttackRange;
        SightRange = character.EntityStats.SightRange;
    }

    public override void LogicUpdate()
    {
        Player = PartyManager.Instance.GetPlayer().gameObject;

        List<EnemyScript> Enemies = GameObject.FindObjectsOfType<EnemyScript>().ToList();

        //Ближайший враг в радиусе
        if (Enemies.Count != 0)
            NearestEnemy = AIUtilities.FindNearestEntity(gameObject.transform, AIUtilities.EnemyTag);

        //Находится ли игрок в радиусе
        if (Player)
            IsPlayerNearby = AIUtilities.IsCertainEntityInRadius(gameObject, Player, 1f);

        enemyFollowing = new TargetFollowingState(NearestEnemy, gameObject, InnerStateMachine);
        playerFollowing = new TargetFollowingState(Player, gameObject, InnerStateMachine);

        if (NearestEnemy == null)
        {
            InnerStateMachine.ChangeState(IsPlayerNearby ? idleState : playerFollowing);
            character.StaminaRecovering();
        }
        else
        {
            IsEnemyNearby = AIUtilities.IsCertainEntityInRadius(gameObject, NearestEnemy, AttackRange);

            InnerStateMachine.ChangeState(IsEnemyNearby ? attackingState : enemyFollowing);
        }

        InnerStateMachine.CurrentState?.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        InnerStateMachine.CurrentState?.PhysicsUpdate();
    }

    public override void ExitState()
    {
        base.ExitState();
        character.IsActive = true;
        playerInput.enabled = true;
        InnerStateMachine.CurrentState.ExitState();
    }
}