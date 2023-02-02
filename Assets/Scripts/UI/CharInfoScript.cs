using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CharInfoScript : MonoBehaviour
{
    Label CharInfo;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        CharInfo = root.Q<Label>("CharInfoLabel");
    }

    private void FixedUpdate()
    {
        CharInfo.text = GetPlayerInfo();
    }

    private string GetPlayerInfo()
    {
        string Info = "Stat\tBase\tCurrent";

        CharacterScript player = GameManager.Instance.partyManager.GetPlayer();

        foreach (var item in player.EntityStats.ModifiableStats)
        {
            if (item.Value != null)
            {
                Info += $"\n{item.Key.ToString()}\t{item.Value.GetClearValue()}\t{item.Value.GetFinalValue()}";
            }
           
        }

        Info += "\n\nElement\tRes\tBonus";

        foreach (var item in player.EntityStats.ElementsResBonus)
        {
            Info += $"\n{item.Key.ToString()}\t{item.Value.Resistance.GetClearValue()}\\{item.Value.Resistance.GetFinalValue()}" +
                $"\t{item.Value.DamageBonus.GetClearValue()}\\{item.Value.DamageBonus.GetFinalValue()}";
        }

        return Info;
    }
}