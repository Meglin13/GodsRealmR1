using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class CharacterInfoControl : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<CharacterInfoControl, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        Image Image = new Image();
        Label Name = new Label("PLACEHOLDER");
        Label Amount = new Label("000 >> 000");

        public CharacterInfoControl()
        {
            AddToClassList("char-info");

            Image.AddToClassList("char-info-icon");
            Name.AddToClassList("char-info-label");
            Amount.AddToClassList("char-info-label-amount");

            Add(Image);
            Add(Name);
            Add(Amount);
        }

        public void SetInfo(Stat value, StatType statType)
        {
            UIManager.Instance.ChangeLabelsText(Name, statType.ToString(), UIManager.Instance.UITable);
            Amount.text = $"{value.GetClearValue()}";
            if (value.GetClearValue() != value.GetFinalValue())
            {
                Amount.text += $" >>> {value.GetFinalValue()}";
            }
        }
    }
}
