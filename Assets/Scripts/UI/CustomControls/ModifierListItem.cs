using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class ModifierListItem : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<ModifierListItem, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        private Image StatIcon;
        private Label StatName;
        private Label AmountOfMod;

        public ModifierListItem()
        {
            StatIcon = new Image();
            Add(StatIcon);
            StatIcon.AddToClassList("modifier-list-item-icon");

            StatName = new Label();
            Add(StatName);
            StatName.AddToClassList("modifier-list-item-stat-label");

            AmountOfMod = new Label();
            Add(AmountOfMod);
            AmountOfMod.AddToClassList("modifier-list-item-amount-label");

            AddToClassList("modifier-list-item");
        }

        public void SetModifier(Modifier modifier)
        {
            string statKey;
            Color col;

            if (modifier.StatType == StatType.Resistance | modifier.StatType == StatType.ElementalDamageBonus)
            {
                if (modifier.StatType == StatType.ElementalDamageBonus)
                    statKey = modifier.Element + "DamageBonus";
                else
                    statKey = modifier.Element + "Resistance";

                StatIcon.sprite = Resources.Load<Sprite>($"Icons/ModifierIcons/{modifier.Element}");

                if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[modifier.Element], out col))
                    StatIcon.tintColor = col;
            }
            else
            {
                if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.StatsColor[modifier.StatType], out col))
                    StatIcon.tintColor = col;

                StatIcon.sprite = Resources.Load<Sprite>($"Icons/ModifierIcons/{modifier.StatType}");

                statKey = modifier.StatType.ToString();
            }

            UIManager.Instance.ChangeLabelsText(StatName, statKey, UIManager.Instance.UITable);

            AmountOfMod.text = modifier.ModifierType == ModType.Buff ? "+" : "-";
            AmountOfMod.text += modifier.Amount.ToString();
            AmountOfMod.style.color = modifier.ModifierType == ModType.Buff ? 
                Color.green : Color.red;
            if (modifier.ModifierAmountType == ModifierAmountType.Procent)
                AmountOfMod.text += "%";
        }
    }
}