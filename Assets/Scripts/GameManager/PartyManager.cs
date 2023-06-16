using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

public enum CommandType { ComeToLeader, GoToPoint, UseSpecial, UseDistract, UseUltimate }

/// <summary>
/// ��� ������������ ����������
/// </summary>
public enum PartySwitchingType
{
    /// <summary> �� ����� ��������� ������ ���� �������� � ����� ���� ����� ������������� </summary>
    Manual,
    /// <summary> ��������� ��������� ��������� �� �����, �� ����� ���� ������ ������������� </summary>
    Leader,
    /// <summary> ��������� ��������� ��������� �� ����� � ����� ���� ����� ������������� </summary>
    Party
}

public class PartyManager : MonoBehaviour, ISingle
{
    public static PartyManager Instance { get; private set; }

    /// <summary>
    /// ������ ������
    /// </summary>
    [ReadOnly]
    public int LeaderIndex = 0;

    /// <summary>
    /// ��� ������������ ����������
    /// </summary>
    public PartySwitchingType switchingType = PartySwitchingType.Manual;

    private GameObject Party;
    public GameObject SpawnPoint;

    /// <summary>
    /// ������ �������� ������ �������
    /// </summary>
    [HideInInspector]
    public List<CharacterScript> PartyMembers = new List<CharacterScript>();


    public void Initialize()
    {
        Instance = this;

        Party = new GameObject("Party");
    }

    /// <summary>
    /// ��������� ���������, ������� ��������� �����
    /// </summary>
    /// <returns>��������</returns>
    public CharacterScript GetPlayer()
    {
        if (PartyMembers.Count > 0)
        {
            return PartyMembers[LeaderIndex];
        }
        return null;
    }

    /// <summary>
    /// ������������ ����������
    /// </summary>
    /// <param _name="CurrentPlayer"></param>
    /// <param _name="newPlayerIndex"></param>
    public void SwitchPlayer(CharacterScript CurrentPlayer, int newPlayerIndex)
    {
        if (newPlayerIndex >= PartyMembers.Count | newPlayerIndex == LeaderIndex | switchingType == PartySwitchingType.Leader)
        {
            return;
        }

        CharacterScript NewPlayer = PartyMembers[newPlayerIndex];
        LeaderIndex = newPlayerIndex;

        if (switchingType == PartySwitchingType.Manual)
        {
            NewPlayer.gameObject.SetActive(true);
            CurrentPlayer.gameObject.SetActive(false);

            NewPlayer.agent.Warp(CurrentPlayer.transform.position);
        }
        else if (switchingType == PartySwitchingType.Party)
        {
            //������ �����
            SidekickState sidekickState = new SidekickState(CurrentPlayer, CurrentPlayer.EntityStateMachine);
            CurrentPlayer.EntityStateMachine.ChangeState(sidekickState);
            CurrentPlayer.IsActive = false;

            //����� �����
            PlayerState playerState = new PlayerState(NewPlayer, NewPlayer.EntityStateMachine);
            NewPlayer.EntityStateMachine.ChangeState(playerState);
            NewPlayer.IsActive = true;
        }

        CameraCenterBehaviour.Instance.SetTarget(NewPlayer.gameObject.transform);
    }

    public void Initialize(List<CharacterScript> characters)
    {
        Initialize();

        if (characters.Count == 0)
        {
            Debug.Log("No chars!!!");
            return;
        }

        foreach (var item in characters)
        {
            var chara = Instantiate(item, SpawnPoint.transform.position, Quaternion.EulerAngles(0, 0, 0), Party.transform);
            PartyMembers.Add(chara);
            chara.gameObject.name = chara.EntityStats.Name;
        }

        foreach (var i in PartyMembers)
            i.HotKeyNumber = PartyMembers.IndexOf(i);

        PartyMembers[0].IsActive = true;
        CameraCenterBehaviour.Instance.SetTarget(PartyMembers[0].transform);

        if (switchingType == PartySwitchingType.Manual)
        {
            foreach (var item in PartyMembers)
            {
                if (!item.IsActive)
                {
                    item.IsActive = true;
                    item.gameObject.SetActive(false);
                }
            }
        }
    }

    public void CharacterDeath(CharacterScript character)
    {
        if (PartyMembers.Count > 1)
        {
            PartyMembers.Remove(character);
            PartyMembers[0].IsActive = true;

            if (switchingType == PartySwitchingType.Party | switchingType == PartySwitchingType.Leader)
            {
                PartyMembers[0].EntityStateMachine.ChangeState(new PlayerState(PartyMembers[0], PartyMembers[0].EntityStateMachine));
            }
            else
            {
                PartyMembers[0].gameObject.SetActive(true);
                PartyMembers[0].agent.Warp(character.transform.position);
            }

            CameraCenterBehaviour.Instance.SetTarget(PartyMembers[0].transform);

            int i = 0;
            foreach (var item in PartyMembers)
            {
                item.HotKeyNumber = i;
                i++;
            }
        }
        else
            UIManager.Instance.OpenMenu(UIManager.Instance.DeathScreen);
    }

    #region [Team Commands]

    public void GiveCommandToMember(CommandType command, int index)
    {
        if (index < PartyMembers.Count & index != LeaderIndex)
        {
            CharacterScript character = PartyMembers[index];

            switch (command)
            {
                case CommandType.UseSpecial:

                    break;

                case CommandType.UseDistract:

                    break;

                case CommandType.UseUltimate:

                    break;
            }
        }
    }

    public void GiveCommandToMember(CommandType command, int index, Vector3 target)
    {
        if (index != LeaderIndex)
        {
            CharacterScript character = PartyMembers[index];
            Debug.Log(command);
            switch (command)
            {
                case CommandType.ComeToLeader:
                    character.EntityStateMachine.CurrentState.InnerStateMachine.
                        ChangeState(new TargetFollowingState(GetPlayer().gameObject, character.gameObject, character.EntityStateMachine.CurrentState.InnerStateMachine));
                    break;

                case CommandType.GoToPoint:
                    Vector3 mousePos = Input.mousePosition;
                    mousePos.z = Camera.main.nearClipPlane;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

                    character.EntityStateMachine.CurrentState.InnerStateMachine.
                        ChangeState(new TargetFollowingState(worldPosition, character.gameObject, character.EntityStateMachine.CurrentState.InnerStateMachine));
                    break;
            }
        }
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
            if (item && item.EntityStats.TeamBuff != null)
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
                GameManager.Instance.inventory.Capacity += (int)Math.Round(modifier.Amount);
                break;

            default:
                foreach (var item in PartyMembers)
                    GameManager.Instance.StartCoroutine(item.AddModifier(modifier));
                break;
        }
    }

    /// <summary>
    /// ����� ��� ���������� ����, �������� ��� ������������
    /// </summary>
    /// <param _name="Amount">����������� ����������</param>
    /// <param _name="type">��� ��������</param>
    public void GiveSupportToAll(float Amount, StatType type)
    {
        foreach (var item in PartyMembers)
        {
            if (type == StatType.Health | type == StatType.Stamina | type == StatType.Mana)
            {
                if (type == StatType.Mana)
                {
                    var ManaRecBonus = Mathf.Clamp(item.EntityStats.ModifiableStats[StatType.ManaRecoveryBonus].GetFinalValue(), 0, 80) / 100;
                    Amount *= (1 + ManaRecBonus);
                }

                item.GiveSupport(Amount, type);

                if (item.gameObject.active)
                {
                    string PopUpColor = GameManager.Instance.colorManager.StatsColor[type];

                    MiscUtilities.DamagePopUp(item.transform, $"+{Math.Round(Amount)}", PopUpColor, 0.8f); 
                }
            }
            else
            {
                Debug.Log("Wrong type of support!!!");
            }
        }
    }

    #endregion [Team Commands]
}