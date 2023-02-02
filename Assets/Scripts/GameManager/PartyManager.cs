using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PartyManager : MonoBehaviour, IManager
{
    public int LeaderIndex = 0;
    public bool CanSwitch = true;
    public CharacterScript Player { get; set; }
    public static PartyManager Instance { get; private set; }

    [HideInInspector]
    public List<CharacterScript> PartyMembers = new List<CharacterScript>();

    //Переключение персонажей
    public void SwitchPlayer(CharacterScript CurrentPlayer, int newPlayerIndex)
    {
        if (newPlayerIndex > PartyMembers.Count | newPlayerIndex > 3 | !CanSwitch)
        {
            Debug.Log("Cannot switch");
            return;
        }

        CharacterScript NewPlayer = PartyMembers[newPlayerIndex - 1];

        //Старый игрок
        SidekickState sidekickState = new SidekickState(CurrentPlayer, CurrentPlayer.CharacterStateMachine);
        CurrentPlayer.CharacterStateMachine.ChangeState(sidekickState);
        CurrentPlayer.IsActive = false;

        //Новый игрок
        PlayerState playerState = new PlayerState(NewPlayer, NewPlayer.CharacterStateMachine);
        NewPlayer.CharacterStateMachine.ChangeState(playerState);
        NewPlayer.IsActive = true;

        LeaderIndex = newPlayerIndex-1;
    }

    public void Initialize()
    {
        Instance = this;
    }

    public void Initialize(List<CharacterScript> characters)
    {
        if (characters.Count == 0)
        {
            Debug.Log("No chars!!!");
            return;
        }

        PartyMembers = characters;

        foreach (var i in PartyMembers)
            i.HotKeyNumber = PartyMembers.IndexOf(i);

        PartyMembers[0].IsActive = true;
    }

    //TODO: Сделать выбор членов команды
    //public void Initialize(List<CharacterScript> characters)
    //{
    //    //Добавление персонажей
    //    PartyMembers = characters;

    //    //Назначение клавиш
    //    foreach (var item in PartyMembers)
    //        item.HotKeyNumber = PartyMembers.IndexOf(item);

    //    PartyMembers[0].IsActive = true;
    //}

    public void CharDeath(CharacterScript character)
    {
        if (PartyMembers.Count > 0)
        {
            PartyMembers.Remove(character);
            PartyMembers[0].IsActive = true;

            PartyMembers[0].CharacterStateMachine.ChangeState(new PlayerState(PartyMembers[0], PartyMembers[0].CharacterStateMachine));

            GameObject.FindObjectOfType<CameraCenterBehaviour>().gameObject.transform.SetParent(PartyMembers[0].transform);

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
                TargetFollowingState comeToPlayer = new TargetFollowingState(PartyMembers[LeaderIndex].gameObject, item.gameObject, item.CharacterStateMachine.CurrentState.InnerStateMachine);
                item.CharacterStateMachine.CurrentState.InnerStateMachine.ChangeState(comeToPlayer);
            }
        }
    }

    public void ApplyTeamBuff()
    {
        foreach (var item in PartyMembers)
        {
            if (item.EntityStats.TeamBuff != null)
            {
                ApplyBuffOnTeam(item.EntityStats.TeamBuff);
            }
        }
    }

    public void ApplyBuffOnTeam(Modifier modifier)
    {
        if (modifier.StatType == StatType.InventorySlots)
            GameManager.Instance.inventory.Capacity += (int)Math.Round(modifier.Amount);
        else
        {
            foreach (var item in PartyMembers)
                StartCoroutine(item.AddModifier(modifier));
        }
    }

    public void GiveHealToAll(float Amount)
    {
        foreach (var item in PartyMembers)
        {
            GiveHealToTeammate(item, Amount);
            MiscUtilities.DamagePopUp(item.transform, $"+{Math.Round(Amount)}", "#45DA00", 0.8f);
        }
    }

    public void GiveHealToTeammate(CharacterScript character, float Amount)
    {
        if (character.CurrentHealth + Amount <= character.EntityStats.ModifiableStats[StatType.Health].GetFinalValue())
            character.CurrentHealth += Amount;
        else
            character.CurrentHealth = character.EntityStats.ModifiableStats[StatType.Health].GetFinalValue();
    }

    public void GiveManaToTeammate(CharacterScript character, float Amount)
    {
        if (character.CurrentMana + Amount <= character.EntityStats.ModifiableStats[StatType.Mana].GetFinalValue())
            character.CurrentMana += Amount;
        else
            character.CurrentMana = character.EntityStats.ModifiableStats[StatType.Mana].GetFinalValue();
    }
    #endregion

    public CharacterScript GetPlayer()
    {
        return PartyMembers[LeaderIndex];
    }
}