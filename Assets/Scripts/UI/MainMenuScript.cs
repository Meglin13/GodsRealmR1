using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuScript : UIScript
    {
        private Button StartButton;
        private Button ExitButton;
        private Button SettingsButton;
        public string NextSceneName = "TestScene";
        public GameObject SettingsMenu;
        public GameObject LoadingScreen;

        internal override void OnBind()
        {
            base.OnBind();

            Debug.Log("MainMenu Enabled");

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

        void StartButtonClicked() => UIManager.Instance.ChangeScene(NextSceneName, gameObject);

        void SettingsButtonClicked()
        {
            UIManager.Instance.ChangeMenu(gameObject, SettingsMenu);
        }

        void ExitButtonClicked() => Application.Quit();
    }
}