using System.Collections.Generic;
using System.Linq;
using UI.CustomControls;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using static MyBox.EditorTools.MyGUI;

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
            var CharacterContainer = root.Q<VisualElement>("CharacterList");

            for (int i = 0; i < AvailableCharacters.Count; i++)
            {
                if (CharacterContainer.childCount - 1 < i)
                {
                    EntityStats stats = null;

                    CharacterSlot characterSlot = new CharacterSlot();

                    if (i < AvailableCharacters.Count)
                    {
                        stats = AvailableCharacters[i];
                        characterSlot.SetSlot(stats);
                        characterSlot.AddManipulator(new Clickable(() => AddCharacter(characterSlot.entityStats)));
                    }

                    CharacterContainer.Add(characterSlot);
                }
                else
                {
                    CharacterContainer.Query<CharacterSlot>().ToList()[i].SetSlot(i < AvailableCharacters.Count ? AvailableCharacters[i] : null);
                }
            }

            var PartyShow = root.Q<VisualElement>("PartyShow").Query<CharacterIcon>().ToList();

            for (int i = 0; i < SelectedCharacters.Capacity; i++)
            {
                CharacterIcon character = PartyShow[i];
                EntityStats chara = null;

                if (i < SelectedCharacters.Count)
                    chara = SelectedCharacters[i];

                character.SetSlot(chara);

                System.Action deleteAction = () =>
                {
                    if (character.entityStats)
                    {
                        SelectedCharacters.Remove(character.entityStats);
                        character.SetSlot(null);
                        LoadInfo();
                    }
                };
                character.DeleteButton.clicked -= deleteAction;
                character.DeleteButton.clicked += deleteAction;
            }
        }

        private void AddCharacter(EntityStats character)
        {
            if (!SelectedCharacters.Contains(character) & SelectedCharacters.Count < 3)
            {
                SelectedCharacters.Add(character);
            }
            else
            {
                SelectedCharacters.Remove(character);
            }

            LoadInfo();
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