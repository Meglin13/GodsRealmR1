using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class CharInfoScript : UIScript
    {
        Label CharInfo;

        internal override void OnBind()
        {
            base.OnBind();

            CharInfo = root.Q<Label>("CharInfoLabel");
        }

        private void FixedUpdate()
        {
            CharInfo.text = GetPlayerInfo();
        }

        private string GetPlayerInfo()
        {
            string Info = "Stat\tBase\tCurrent";

            CharacterScript player = GameManager.GetInstance().partyManager.GetPlayer();

            foreach (var item in player.EntityStats.ModifiableStats)
            {
                if (item.Value != null)
                {
                    Info += $"\n{item.Key}\t{item.Value.GetClearValue()}\t{item.Value.GetFinalValue()}";
                }

            }

            Info += "\n\nElement\tRes\tBonus";

            foreach (var item in player.EntityStats.ElementsResBonus)
            {
                Info += $"\n{item.Key}\t{item.Value.Resistance.GetClearValue()}\\{item.Value.Resistance.GetFinalValue()}" +
                    $"\t{item.Value.DamageBonus.GetClearValue()}\\{item.Value.DamageBonus.GetFinalValue()}";
            }

            return Info;
        }
    }
}