using System.Collections.Generic;
using System.Linq;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PartySetupScript : UIScript
    {
        private List<EntityStats> AvailableCharacters;

        private List<EntityStats> SelectedCharacters = new List<EntityStats>(3);

        private Button StartRunBT;

        internal override void OnBind()
        {
            base.OnBind();

            AvailableCharacters = Resources.LoadAll("ScriptableObjects/Character", typeof(EntityStats)).Cast<EntityStats>().Where(x => x.IsUnlocked).ToList();

            StartRunBT = root.Q<Button>("StartBT");
            StartRunBT.clicked += StartRun;

            LoadInfo();
        }

        private void LoadInfo()
        {
            var CharacterList = root.Q<VisualElement>("CharacterList");
            CharacterList.Clear();

            foreach (var item in AvailableCharacters)
            {
                CharacterSlot characterSlot = new CharacterSlot();

                characterSlot.SetSlot(item);
                characterSlot.AddManipulator(new Clickable(() => AddCharacter(characterSlot.entityStats)));

                CharacterList.Add(characterSlot);
            }

            var PartyShow = root.Q<VisualElement>("PartyShow");
            PartyShow.Clear();

            for (int i = 0; i < SelectedCharacters.Capacity; i++)
            {
                CharacterIcon character = new CharacterIcon();
                PartyShow.Add(character);

                EntityStats chara = null;

                if (i < SelectedCharacters.Count)
                    chara = SelectedCharacters[i];

                character.SetSlot(chara);

                character.DeleteButton.clicked += () =>
                {
                    if (character.entityStats)
                    {
                        SelectedCharacters.Remove(character.entityStats);
                        character.entityStats = null;
                    }
                };
            }
        }

        private void AddCharacter(EntityStats character)
        {
            if (!SelectedCharacters.Contains(character) & SelectedCharacters.Count < 3)
            {
                SelectedCharacters.Add(character);
                LoadInfo();
            }
            else
            {
                SelectedCharacters.Remove(character);
            }
        }

        private void StartRun()
        {
            if (SelectedCharacters.Count == 0)
            {
                UIManager.Instance.ShowModalWindow(ModalWindowType.OK, "CANT_START_RUN_CAPTION", "CANT_START_RUN_TITLE");
            }
            else
            {
                List<CharacterScript> subListObjects = Resources.LoadAll("Prefabs/Character", typeof(CharacterScript)).Cast<CharacterScript>().ToList();

                List<CharacterScript> characters = new List<CharacterScript>();

                foreach (var item in subListObjects)
                {
                    foreach (var character in SelectedCharacters)
                    {
                        if (item.EntityStats == character)
                            characters.Add(item);
                    }
                }

                GameManager.Instance.SetCharacters(characters);
                UIManager.Instance.ChangeScene("RunScene", gameObject);
            }
        }

        public void OnDisable()
        {
            StartRunBT.clicked -= StartRun;
        }
    }
}