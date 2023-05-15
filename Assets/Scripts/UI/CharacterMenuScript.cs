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
                characterSlot.AddManipulator(new Clickable(()=> LoadInfo(characterSlot.entityStats)));
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

            Foldout stats = root.Q<Foldout>("Stats");
            stats.Clear();
           
            if (character.ModifiableStats == null)
            {
                character.Initialize(character.Level);
            }

            Foldout skills = root.Q<Foldout>("Skills");
            skills.Clear();

            foreach (var item in character.ModifiableStats)
            {
                CharacterInfoControl characterInfoControl = new CharacterInfoControl();
                characterInfoControl.SetInfo(item.Value, item.Key);
                stats.Add(characterInfoControl);
            }

            foreach (var item in character.SkillSet)
            {
                var name = UIManager.Instance.GetLocalizedString(item.Value.Name, CharacterTable);

                var desc = UIManager.Instance.GetLocalizedString(item.Value.Description, CharacterTable);

                Label lab = new Label($"{name}\n{desc}\n");
                lab.style.whiteSpace = WhiteSpace.Normal;
                skills.Add(lab);
            }
        }
    }
}
