using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour, IManager
{
    /// <summary>
    /// ������ ������
    /// </summary>
    public int LeaderIndex = 0;

    /// <summary>
    /// ����������, ����� �� �������������
    /// </summary>
    public bool CanSwitch = true;
    public CharacterScript Player { get; set; }
    public static PartyManager Instance { get; private set; }

    /// <summary>
    /// ������ �������� ������ �������
    /// </summary>
    [HideInInspector]
    public List<CharacterScript> PartyMembers = new List<CharacterScript>();

    /// <summary>
    /// ������������ ����������
    /// </summary>
    /// <param name="CurrentPlayer"></param>
    /// <param name="newPlayerIndex"></param>
    public void SwitchPlayer(CharacterScript CurrentPlayer, int newPlayerIndex)
    {
        if (newPlayerIndex > PartyMembers.Count | newPlayerIndex > 3 | !CanSwitch)
        {
            Debug.Log("Cannot switch");
            return;
        }

        CharacterScript NewPlayer = PartyMembers[newPlayerIndex - 1];

        //������ �����
        SidekickState sidekickState = new SidekickState(CurrentPlayer, CurrentPlayer.EntityStateMachine);
        CurrentPlayer.EntityStateMachine.ChangeState(sidekickState);
        CurrentPlayer.IsActive = false;

        //����� �����
        PlayerState playerState = new PlayerState(NewPlayer, NewPlayer.EntityStateMachine);
        NewPlayer.EntityStateMachine.ChangeState(playerState);
        NewPlayer.IsActive = true;

        LeaderIndex = newPlayerIndex - 1;
        CameraCenterBehaviour.Instance.SetTarget(NewPlayer.gameObject.transform);
    }

    public void Initialize()
    {
        Instance = this;
    }

    //TODO: ������� ����� ������ �������
    public void Initialize(List<CharacterScript> characters)
    {
        Initialize();

        if (characters.Count == 0)
        {
            Debug.Log("No chars!!!");
            return;
        }

        PartyMembers = characters;

        foreach (var i in PartyMembers)
            i.HotKeyNumber = PartyMembers.IndexOf(i);

        PartyMembers[0].IsActive = true;
        CameraCenterBehaviour.Instance.SetTarget(PartyMembers[0].transform);
    }


    public void CharDeath(CharacterScript character)
    {
        if (PartyMembers.Count > 0)
        {
            PartyMembers.Remove(character);
            PartyMembers[0].IsActive = true;

            PartyMembers[0].EntityStateMachine.ChangeState(new PlayerState(PartyMembers[0], PartyMembers[0].EntityStateMachine));

            CameraCenterBehaviour.Instance.SetTarget(PartyMembers[0].transform);

            int i = 0;
            foreach (var item in PartyMembers)
            {
                item.HotKeyNumber = i;
                i++;
            }
        }
        else
        {
            //TODO: ����� ������
        }
    }

    #region [Team Commands]

    public void GiveCommandToMember()
    {
        //TODO: ����������� ������� ���������
    }

    //TODO: ����� �������
    public void ComeToPlayer()
    {
        foreach (var item in PartyMembers)
        {
            if (!item.IsActive)
            {
                TargetFollowingState comeToPlayer = new TargetFollowingState(PartyMembers[LeaderIndex].gameObject, item.gameObject, item.EntityStateMachine.CurrentState.InnerStateMachine);
                item.EntityStateMachine.CurrentState.InnerStateMachine.ChangeState(comeToPlayer);
            }
        }
    }

    public void ApplyTeamBuff()
    {
        foreach (var item in PartyMembers)
        {
            if (item && item?.EntityStats.TeamBuff != null)
            {
                ApplyBuffOnTeam(item.EntityStats.TeamBuff);
            }
        }
    }

    public void ApplyBuffOnTeam(Modifier modifier)
    {
        switch (modifier.StatType)
        {
            case StatType.InventorySlots:
                GameManager.GetInstance().inventory.Capacity += (int)Math.Round(modifier.Amount);
                break;

            case StatType.WeaponLength:
                break;

            default:
                foreach (var item in PartyMembers)
                    StartCoroutine(item.AddModifier(modifier));
                break;
        }
    }

    /// <summary>
    /// ����� ��� ���������� ����, �������� ��� ������������
    /// </summary>
    /// <param name="Amount">����������� ����������</param>
    /// <param name="type">��� ��������</param>
    public void GiveSupportToAll(float Amount, StatType type)
    {
        foreach (var item in PartyMembers)
        {
            if (type == StatType.Health | type == StatType.Stamina | type == StatType.Mana)
            {
                item.GiveSupport(Amount, type);

                string PopUpColor = GameManager.GetInstance().colorManager.StatsColor[type];

                MiscUtilities.DamagePopUp(item.transform, $"+{Math.Round(Amount)}", PopUpColor, 0.8f);
            }
            else
            {
                Debug.Log("Wrong type to support!!!");
            }
        }
    }

    public void GiveManaToTeammate(CharacterScript character, float Amount, StatType type)
    {
        character.GiveSupport(Amount, type);
    }

    #endregion [Team Commands]

    /// <summary>
    /// ��������� ���������, ������� ��������� �����
    /// </summary>
    /// <returns>��������</returns>
    public CharacterScript GetPlayer()
    {
        return PartyMembers[LeaderIndex];
    }
}