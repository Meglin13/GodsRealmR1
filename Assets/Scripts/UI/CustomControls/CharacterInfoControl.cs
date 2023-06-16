using UnityEngine;
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
        public Label Name = new Label("PLACEHOLDER");
        public Label Amount = new Label("000 >> 000");

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

            Image.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Icons/ModifierIcons/{statType}"));

            if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.StatsColor[statType], out Color col))
                Image.style.unityBackgroundImageTintColor = col;
        }

        public void SetInfo(Stat value, Element element)
        {
            UIManager.Instance.ChangeLabelsText(Name, element.ToString(), UIManager.Instance.UITable);
            Amount.text = $"{value.GetClearValue()}";
            if (value.GetClearValue() != value.GetFinalValue())
            {
                Amount.text += $" >> {value.GetFinalValue()}";
            }

            Image.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Icons/ModifierIcons/{element}"));

            if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[element], out Color col))
                Image.style.unityBackgroundImageTintColor = col;
        }
    }
}
