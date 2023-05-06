using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuScript : UIScript
    {
        private Button StartButton;
        private Button ExitButton;
        private Button SettingsButton;
        public GameObject SettingsMenu;
        public GameObject LoadingScreen;

        internal override void OnBind()
        {
            base.OnBind();

            StartButton = root.Q<Button>("StartBT");
            ExitButton = root.Q<Button>("ExitBT");
            SettingsButton = root.Q<Button>("SettingsBT");

            StartButton.clicked += StartButtonClicked;
            SettingsButton.clicked += SettingsButtonClicked;
            ExitButton.clicked += ExitButtonClicked;
        }

        internal void OnDisable()
        {
            StartButton.clicked -= StartButtonClicked;
            SettingsButton.clicked -= SettingsButtonClicked;
            ExitButton.clicked -= ExitButtonClicked;
        }

        void StartButtonClicked()
        {
            if (SaveLoadSystem.SaveLoadSystem.SavedData == null)
                manager.ChangeScene("TutorialScene", gameObject);
            else
                manager.ChangeScene("HubScene", gameObject);
        }

        void SettingsButtonClicked() => manager.ChangeMenu(gameObject, SettingsMenu);

        void ExitButtonClicked() => Application.Quit();
    }
}