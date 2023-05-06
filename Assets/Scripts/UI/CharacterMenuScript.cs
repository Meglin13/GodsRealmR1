using System.Collections.Generic;
using System.Linq;
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

            foreach (var character in AvailableCharacters)
            {

            }
        }

        private void LoadInfo(EntityStats character)
        {
            VisualElement characterArt = root.Q<VisualElement>("CharacterArt");
            characterArt.style.backgroundImage = new StyleBackground(character.Art);
        }
    }
}
