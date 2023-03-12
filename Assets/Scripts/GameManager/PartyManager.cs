using System;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour, IManager
{
    /// <summary>
    /// Индекс лидела
    /// </summary>
    public int LeaderIndex = 0;

    /// <summary>
    /// Определяет, можно ли переключаться
    /// </summary>
    public bool CanSwitch = true;
    public CharacterScript Player { get; set; }
    public static PartyManager Instance { get; private set; }

    /// <summary>
    /// Список активных членов команды
    /// </summary>
    [HideInInspector]
    public List<CharacterScript> PartyMembers = new List<CharacterScript>();

    /// <summary>
    /// Переключение персонажей
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

        //Старый игрок
        SidekickState sidekickState = new SidekickState(CurrentPlayer, CurrentPlayer.EntityStateMachine);
        CurrentPlayer.EntityStateMachine.ChangeState(sidekickState);
        CurrentPlayer.IsActive = false;

        //Новый игрок
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

    //TODO: Сделать выбор членов команды
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
            //TODO: ЭКРАН СМЕРТИ
        }
    }

    #region [Team Commands]

    public void GiveCommandToMember()
    {
        //TODO: Реализовать команды союзникам
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
    /// Метод для добавления маны, здоровья или выносливости
    /// </summary>
    /// <param name="Amount">Добавляемое количество</param>
    /// <param name="type">Тип атрибута</param>
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
    /// Получение персонажа, которым управляет игрок
    /// </summary>
    /// <returns>Персонаж</returns>
    public CharacterScript GetPlayer()
    {
        return PartyMembers[LeaderIndex];
    }
}