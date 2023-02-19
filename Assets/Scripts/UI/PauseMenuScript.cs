using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenuScript : UIScript
    {
        private Button ResumeButton;
        private Button RestartButton;
        private Button RunStatsButton;
        private Button MainMenuButton;
        private Button SettingsButton;

        private string CurrentSceneName;

        internal override void OnBind()
        {
            base.OnBind();

            CurrentSceneName = SceneManager.GetActiveScene().name;

            ResumeButton = root.Q<Button>("ResumeBT");
            RestartButton = root.Q<Button>("RestartBT");
            RunStatsButton = root.Q<Button>("RunStatsBT");
            SettingsButton = root.Q<Button>("SettingsBT");
            MainMenuButton = root.Q<Button>("MainMenuBT");

            ResumeButton.clicked += ResumeButtonClicked;
            RestartButton.clicked += RestartButtonClicked;
            MainMenuButton.clicked += MainMenuButtonClicked;
            SettingsButton.clicked += SettingsButtonClicked;
            RunStatsButton.clicked += RunButtonClicked;
        }

        void OnDisable()
        {
            Time.timeScale = 1;

            ResumeButton.clicked -= ResumeButtonClicked;
            RestartButton.clicked -= RestartButtonClicked;
            MainMenuButton.clicked -= MainMenuButtonClicked;
            SettingsButton.clicked -= SettingsButtonClicked;
            RunStatsButton.clicked += RunButtonClicked;
        }

        void ResumeButtonClicked() => UIManager.Instance.GoBack();

        void RunButtonClicked()
        {

        }

        void RestartButtonClicked() => UIManager.Instance.ChangeScene(CurrentSceneName, gameObject);

        void MainMenuButtonClicked() => UIManager.Instance.ChangeScene("MainMenu", gameObject);

        void SettingsButtonClicked() => UIManager.Instance.ChangeMenu(gameObject, UIManager.Instance.SettingsMenu);
    }
}