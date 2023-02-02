using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    private Button ResumeButton;
    private Button RestartButton;
    private Button RunStatsButton;
    private Button MainMenuButton;
    private Button SettingsButton;

    private string CurrentSceneName;

    void OnEnable()
    {
        Time.timeScale = 0;

        CurrentSceneName = SceneManager.GetActiveScene().name;

        var root = GetComponent<UIDocument>().rootVisualElement;

        ResumeButton = root.Q<Button>("ResumeBT");
        RestartButton = root.Q<Button>("RestartBT");
        RunStatsButton = root.Q<Button>("RunStatsBT");
        SettingsButton = root.Q<Button>("SettingsBT");
        MainMenuButton = root.Q<Button>("MainMenuBT");

        ResumeButton.clicked += ResumeButtonClicked;
        RestartButton.clicked += RestartButtonClicked;
        MainMenuButton.clicked += MainMenuButtonClicked;
        SettingsButton.clicked += SettingsButtonClicked;
        //ResumeButton.clicked += ResumeButtonClicked;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
    }

    void ResumeButtonClicked()
    {
        gameObject.SetActive(false);
    }

    void RestartButtonClicked() => UIManager.Instance.ChangeScene(CurrentSceneName, gameObject);

    void MainMenuButtonClicked() => UIManager.Instance.ChangeScene("MainMenu", gameObject);

    void SettingsButtonClicked() => UIManager.Instance.ChangeMenu(gameObject, UIManager.Instance.SettingsMenu, true);
}