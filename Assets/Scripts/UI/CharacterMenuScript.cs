using System.Collections.Generic;
using System.Linq;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class CharacterMenuScript : UIScript
    {
        private List<EntityStats> AvailableCharacters;

        internal override void OnBind()
        {
            base.OnBind();

            AvailableCharacters = Resources.LoadAll("ScriptableObjects/Character", typeof(EntityStats)).Cast<EntityStats>().Where(x => x.IsUnlocked).ToList();

            var CharList = root.Q<VisualElement>("CharacterList");
            CharList.Clear();

            foreach (var character in AvailableCharacters)
            {
                CharacterSlot characterSlot = new CharacterSlot();
                characterSlot.SetSlot(character);
                characterSlot.AddManipulator(new Clickable(() => LoadInfo(characterSlot.entityStats)));
                CharList.Add(characterSlot);
            }

            LoadInfo(AvailableCharacters[0]);
        }

        private void LoadInfo(EntityStats character)
        {
            VisualElement characterArt = root.Q<VisualElement>("CharacterArt");
            characterArt.style.backgroundImage = new StyleBackground(character.Art);

            Label nameLB = root.Q<Label>("CharacterName");
            UIManager.Instance.ChangeLabelsText(nameLB, character.Name, CharacterTable);

            Label descLB = root.Q<Label>("DescLB");
            UIManager.Instance.ChangeLabelsText(descLB, character.Description, CharacterTable);

            VisualElement element = root.Q<VisualElement>("Element");
            element.style.backgroundImage = new StyleBackground(Resources.Load<Sprite>($"Icons/ModifierIcons/{character.Element}"));

            if (ColorUtility.TryParseHtmlString(GameManager.Instance.colorManager.ElementColor[character.Element], out Color col))
                element.style.unityBackgroundImageTintColor = col;

            Foldout stats = root.Q<Foldout>("Stats");
            stats.Clear();

            if (character.ModifiableStats == null)
            {
                character.Initialize(character.Level);
            }

            foreach (var item in character.ModifiableStats)
            {
                CharacterInfoControl characterInfoControl = new CharacterInfoControl();
                characterInfoControl.SetInfo(item.Value, item.Key);
                stats.Add(characterInfoControl);
            }

            Foldout skills = root.Q<Foldout>("Skills");
            skills.Clear();

            foreach (var item in character.SkillSet)
            {
                var name = UIManager.Instance.GetLocalizedString(item.Value.Name, CharacterTable);

                var desc = UIManager.Instance.GetLocalizedString(item.Value.Description, CharacterTable);

                if (item.Value.Name != string.Empty)
                {
                    Label lab = new Label($"<color=#FFC700>{name}</color>\n{desc}");
                    lab.AddToClassList("skill-label");
                    skills.Add(lab);
                }
            }

            Foldout elements = root.Q<Foldout>("Elements");
            elements.Clear();

            CharacterInfoControl infoElements = new CharacterInfoControl();
            UIManager.Instance.ChangeLabelsText(infoElements.Name, "Element", UITable);
            var rb = UIManager.Instance.GetLocalizedString("Resistance", UITable) + "/" + UIManager.Instance.GetLocalizedString("DamageBonus", UITable);
            infoElements.Amount.text = rb;

            elements.Add(infoElements);

            foreach (var item in character.ElementsResBonus)
            {
                CharacterInfoControl characterInfoControl = new CharacterInfoControl();
                characterInfoControl.SetInfo(item.Value.Resistance, item.Key);
                UIManager.Instance.ChangeLabelsText(characterInfoControl.Name, item.Key.ToString(), UITable);
                characterInfoControl.Amount.text = $"{item.Value.Resistance.GetFinalValue()}%/{item.Value.DamageBonus.GetFinalValue()}%";
                elements.Add(characterInfoControl);
            }
        }
    }
}
