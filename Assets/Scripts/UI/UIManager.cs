using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    [RequireComponent(typeof(PlayerInput))]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public GameObject PauseMenu;
        public GameObject SettingsMenu;
        public GameObject LoadingScreen;
        public GameObject Inventory;
        public GameObject InGameUI;

        private GameObject PreviousMenu;
        private GameObject CurrentMenu;

        public bool IsPausable;
        public GameObject Blur;

        public event Action OnMenuOpen = delegate { };
        public event Action OnMenuClose = delegate { };

        private List<GameObject> AvailableUI;

        private void Awake()
        {
            Instance = this;



            var playerInput = GetComponent<PlayerInput>();

            var ExitButton = playerInput.actions["Exit"];
            var InventoryButton = playerInput.actions["Inventory"];

            ExitButton.performed += ctx =>
            {
                if(CurrentMenu == null)
                {
                    this.OpenMenu(PauseMenu);
                }
                else
                {
                    GoBack();
                }
            };

            InventoryButton.performed += ctx => this.OpenMenu(Inventory);

            OnMenuOpen += () =>
            {
                Blur.SetActive(true);
                InGameUI.SetActive(false);
                Time.timeScale = 0;
            };

            OnMenuClose += () =>
            {
                Blur.SetActive(false);
                InGameUI.SetActive(true);
                Time.timeScale = 1;
            };
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
            LoadingScreen.GetComponent<LoadingScreenScript>().LoadScene(SceneName);
        }

        public void GoBack()
        {
            CurrentMenu.SetActive(false);
            if (PreviousMenu != null)
            {
                PreviousMenu.SetActive(true);
                CurrentMenu = PreviousMenu;
                PreviousMenu = null;
            }
            else
            {
                CurrentMenu = null;
                OnMenuClose();
            }
        }

        public void OpenMenu(GameObject menu)
        {
            if (menu == null) GoBack();

            if (!menu.active & CurrentMenu != menu & PreviousMenu != menu)
            {
                menu.SetActive(true);
                CurrentMenu = menu;
                OnMenuOpen();
            }
            else
            {
                GoBack();
            }
        }
    }
}