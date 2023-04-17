using UnityEditor.Search;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenuScript : UIScript
    {
        private Button RestartBT;
        private Button RunStatsBT;
        private Button MainMenuBT;
        private Button SettingsBT;
        private Button ReturnToHubBT;

        private string CurrentSceneName;

        internal override void OnBind()
        {
            base.OnBind();

            CurrentSceneName = SceneManager.GetActiveScene().name;

            RestartBT = root.Q<Button>("RestartBT");
            RunStatsBT = root.Q<Button>("RunStatsBT");
            SettingsBT = root.Q<Button>("SettingsBT");
            MainMenuBT = root.Q<Button>("MainMenuBT");
            ReturnToHubBT = root.Q<Button>("ReturnToHubBT");

            RestartBT.clicked += RestartButtonClicked;
            MainMenuBT.clicked += MainMenuButtonClicked;
            SettingsBT.clicked += SettingsButtonClicked;
            RunStatsBT.clicked += RunButtonClicked;

            if (CurrentSceneName == "HubScene")
            {
                RestartBT.style.display = DisplayStyle.None;
                ReturnToHubBT.style.display = DisplayStyle.None;
                RunStatsBT.style.display = DisplayStyle.None;
            }
        }

        void OnDisable()
        {
            RestartBT.clicked -= RestartButtonClicked;
            MainMenuBT.clicked -= MainMenuButtonClicked;
            SettingsBT.clicked -= SettingsButtonClicked;
            RunStatsBT.clicked -= RunButtonClicked;
        }

        void RunButtonClicked()
        {

        }

        void RestartButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        void MainMenuButtonClicked() => manager.ChangeScene("MainMenu", gameObject);

        void SettingsButtonClicked() => manager.ChangeMenu(gameObject, manager.SettingsMenu);
    }
}