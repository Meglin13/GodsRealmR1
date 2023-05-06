using DialogueSystem;
using System;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

namespace UI
{
    public enum ModalWindowType { YesNo, OKCancel, OK }

    [RequireComponent(typeof(PlayerInput))]
    public class UIManager : MonoBehaviour
    {
        //Localization
        [SerializeField]
        private LocalizedStringTable UILocalTable;
        [SerializeField]
        private LocalizedStringTable ItemsLocalTable;
        [SerializeField]
        private LocalizedStringTable CharacterLocalTable;
        [SerializeField]
        private LocalizedStringTable DialogueLocalTable; 
        [SerializeField]
        private LocalizedStringTable TutorialLocalTable;

        [HideInInspector]
        public StringTable UITable;
        [HideInInspector]
        public StringTable ItemsTable;
        [HideInInspector]
        public StringTable CharacterTable;
        [HideInInspector]
        public StringTable DialogueTable;
        [HideInInspector]
        public StringTable TutorialTable;

        public static UIManager Instance;
        public GameObject PauseMenu;
        public GameObject SettingsMenu;
        public GameObject LoadingScreen;
        public GameObject Inventory;
        public GameObject InGameUI;
        public UIDocument ModalWindow;
        public GameObject DialogueWindow;
        public GameObject DeathScreen;
        public GameObject TutorialScreen;

        private GameObject PreviousMenu;
        public GameObject CurrentMenu;

        private bool CanOpenMenus = true;
        public bool IsModalWindow = false;
        public GameObject Blur;

        private InputAction ExitButton;
        private InputAction InventoryButton;

        public event Action OnMenuOpen = delegate { };
        public event Action OnMenuClose = delegate { };

        private void Awake()
        {
            Instance = this;

            CanOpenMenus = true;

            PlayerInput playerInput = GetComponent<PlayerInput>();

            ExitButton = playerInput.actions["Exit"];
            InventoryButton = playerInput.actions["Inventory"];

            ExitButton.performed += ExitButton_performed;
            InventoryButton.performed += ctx => OpenMenu(Inventory);

            OnMenuOpen += () => SetMenus(true);
            OnMenuClose += () => SetMenus(false);

            //Localization
            UITable = UILocalTable.GetTable();
            CharacterTable = CharacterLocalTable.GetTable();
            ItemsTable = ItemsLocalTable.GetTable();
            DialogueTable = DialogueLocalTable.GetTable();
            TutorialTable = TutorialLocalTable.GetTable();
        }

        private void OnDestroy()
        {
            InventoryButton.performed -= ctx => OpenMenu(Inventory);
            OnMenuClose -= () => SetMenus(false);
            OnMenuOpen -= () => SetMenus(true);
            ExitButton.performed -= ExitButton_performed;

        }

        private void ExitButton_performed(InputAction.CallbackContext obj)
        {
            if (!IsModalWindow)
            {
                if (CurrentMenu == null)
                    OpenMenu(PauseMenu);
                else
                    GoBack();
            }
        }

        void SetMenus(bool IsOpen)
        {
            Blur.SetActive(IsOpen);
            InGameUI.SetActive(!IsOpen);
            Time.timeScale = IsOpen ? 0 : 1;
        }

        public void ChangeMenu(GameObject CurrentMenu, GameObject NextMenu)
        {
            CurrentMenu.SetActive(false);
            NextMenu.SetActive(true);

            this.PreviousMenu = CurrentMenu;
            this.CurrentMenu = NextMenu;
        }

        public void ChangeScene(string SceneName, GameObject PreviousMenu)
        {
            PreviousMenu.SetActive(false);
            LoadingScreen.SetActive(true);

            this.PreviousMenu = null;
            this.CurrentMenu = null;

            CanOpenMenus = false;

            LoadingScreen.GetComponent<LoadingScreenScript>().LoadScene(SceneName);
        }

        public void GoBack()
        {
            if (CurrentMenu)
                CurrentMenu.SetActive(false); 

            if (PreviousMenu != null & !IsModalWindow)
            {
                PreviousMenu.SetActive(true);
                CurrentMenu = PreviousMenu;
                PreviousMenu = null;
            }
            else if (IsModalWindow)
            {
                ModalWindow.rootVisualElement.Q<ModalWindow>().style.display = DisplayStyle.None;
                CurrentMenu.SetActive(true);
            }
            else
            {
                CurrentMenu = null;
                CanOpenMenus = true;
                OnMenuClose();
            }
        }

        public void OpenMenu(GameObject menu, bool canOpenMenu = true)
        {
            if (menu == null)
                GoBack();

            if (CanOpenMenus)
            {
                CanOpenMenus = canOpenMenu;

                if (!menu.active & CurrentMenu != menu & PreviousMenu != menu)
                {
                    menu.SetActive(true);
                    CurrentMenu = menu;
                    OnMenuOpen();
                }
                else
                    GoBack();
            }
        }

        /// <summary>
        /// Установка нового текста и его локализация
        /// </summary>
        /// <param _name="textElement">Текстовый элемент</param>
        /// <param _name="key">Ключ в таблице локализации</param>
        /// <param _name="table">Таблица локализации</param>
        public void ChangeLabelsText(TextElement textElement, string key, StringTable table)
        {
            if (!string.IsNullOrEmpty(key))
            {
                StringTableEntry entry = table[key];
                if (entry != null)
                    textElement.text = entry.LocalizedValue;
                else
                    textElement.text = key;
            }
        }

        public string GetLocalizedString(string key, StringTable table)
        {
            if (!string.IsNullOrEmpty(key))
            {
                StringTableEntry entry = table[key];
                if (entry != null)
                    return entry.LocalizedValue;
            }
            return key;
        }

        public void ShowModalWindow(ModalWindowType type, string Caption, string Title, Action Success = null)
        {
            ModalWindow.gameObject.SetActive(false);
            ModalWindow.gameObject.SetActive(true);

            IsModalWindow = true;

            ModalWindow.rootVisualElement.Q<ModalWindow>().Show(type, Caption, Title, Success, CurrentMenu);
        }
    }
}