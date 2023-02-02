using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    private Button StartButton;
    private Button ExitButton;
    private Button SettingsButton;
    public string SceneName = "TestScene";
    public GameObject SettingsMenu;
    public GameObject LoadingScreen;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        StartButton = root.Q<Button>("StartBT");
        ExitButton = root.Q<Button>("ExitBT");
        SettingsButton = root.Q<Button>("SettingsBT");

        StartButton.clicked += StartButtonClicked;
        SettingsButton.clicked += SettingsButtonClicked;
        ExitButton.clicked += ExitButtonClicked;
    }

    void StartButtonClicked()
    {
        UIManager.Instance.ChangeScene(SceneName, gameObject);
    }

    void SettingsButtonClicked()
    {
        UIManager.Instance.ChangeMenu(gameObject, SettingsMenu, false);
    }

    void ExitButtonClicked()
    {
        Application.Quit();
    }
}