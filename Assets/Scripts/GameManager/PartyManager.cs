using MyBox;
using System;
using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.UIElements;

public enum CommandType { ComeToLeader, GoToPoint, UseSpecial, UseDistract, UseUltimate }

/// <summary>
/// Тип переключения персонажей
/// </summary>
public enum PartySwitchingType
{
    /// <summary> На сцене находится только один персонаж и между ними можно переключаться </summary>
    Manual,
    /// <summary> Персонажи постоянно находятся на сцене, но между ними нельзя переключаться </summary>
    Leader,
    /// <summary> Персонажи постоянно находятся на сцене и между ними можно переключаться </summary>
    Party
}

public class PartyManager : MonoBehaviour, ISingle
{
    public static PartyManager Instance { get; private set; }

    /// <summary>
    /// Индекс лидела
    /// </summary>
    [ReadOnly]
    public int LeaderIndex = 0;

    /// <summary>
    /// Тип переключения персонажей
    /// </summary>
    public PartySwitchingType switchingType = PartySwitchingType.Manual;

    private GameObject Party;
    public GameObject SpawnPoint;

    /// <summary>
    /// Список активных членов команды
    /// </summary>
    [HideInInspector]
    public List<CharacterScript> PartyMembers = new List<CharacterScript>();


    public void Initialize()
    {
        Instance = this;

        Party = new GameObject("Party");
    }

    /// <summary>
    /// Получение персонажа, которым управляет игрок
    /// </summary>
    /// <returns>Персонаж</returns>
    public CharacterScript GetPlayer()
    {
        return PartyMembers[LeaderIndex];
    }

    /// <summary>
    /// Переключение персонажей
    /// </summary>
    /// <param _name="CurrentPlayer"></param>
    /// <param _name="newPlayerIndex"></param>
    public void SwitchPlayer(CharacterScript CurrentPlayer, int newPlayerIndex)
    {
        if (newPlayerIndex > PartyMembers.Count - 1 | newPlayerIndex == LeaderIndex | switchingType == PartySwitchingType.Leader)
        {
            return;
        }

        CharacterScript NewPlayer = PartyMembers[newPlayerIndex];

        if (switchingType == PartySwitchingType.Manual)
        {
            NewPlayer.gameObject.SetActive(true);
            CurrentPlayer.gameObject.SetActive(false);

            NewPlayer.transform.SetPositionAndRotation(CurrentPlayer.transform.position, CurrentPlayer.transform.rotation);

            NewPlayer.agent.Warp(CurrentPlayer.transform.position);
        }
        else if (switchingType == PartySwitchingType.Party)
        {
            //Старый игрок
            SidekickState sidekickState = new SidekickState(CurrentPlayer, CurrentPlayer.EntityStateMachine);
            CurrentPlayer.EntityStateMachine.ChangeState(sidekickState);
            CurrentPlayer.IsActive = false;

            //Новый игрок
            PlayerState playerState = new PlayerState(NewPlayer, NewPlayer.EntityStateMachine);
            NewPlayer.EntityStateMachine.ChangeState(playerState);
            NewPlayer.IsActive = true;
        }

        LeaderIndex = newPlayerIndex;

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
        {
            //TODO: ЭКРАН СМЕРТИ
            UIManager.Instance.OpenMenu(UIManager.Instance.DeathScreen);
        }
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

    //TODO: Общие команды
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
                GameManager.Instance.inventory.Capacity += (int)Math.Round(modifier.Amount);
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
    /// Метод для добавления маны, здоровья или выносливости
    /// </summary>
    /// <param _name="Amount">Добавляемое количество</param>
    /// <param _name="type">Тип атрибута</param>
    public void GiveSupportToAll(float Amount, StatType type)
    {
        foreach (var item in PartyMembers)
        {
            if (type == StatType.Health | type == StatType.Stamina | type == StatType.Mana)
            {
                item.GiveSupport(Amount, type);

                string PopUpColor = GameManager.Instance.colorManager.StatsColor[type];

                MiscUtilities.DamagePopUp(item.transform, $"+{Math.Round(Amount)}", PopUpColor, 0.8f);
            }
            else
            {
                Debug.Log("Wrong type of support!!!");
            }
        }
    }

    public void GiveManaToTeammate(CharacterScript character, float Amount, StatType type)
    {
        character.GiveSupport(Amount, type);
    }

    #endregion [Team Commands]
}