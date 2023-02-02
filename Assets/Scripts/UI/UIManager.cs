using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject LoadingScreen;

    private GameObject PreviousMenu;
    private GameObject CurrentMenu;

    public bool IsPausable;

    //TODO: Сделать кнопку назад
    private void Awake()
    {
        Instance = this;

        var ExitButton = GetComponent<PlayerInput>().actions["Exit"];
        ExitButton.performed += ctx =>
        {
            if (!PauseMenu.active | CurrentMenu == PauseMenu)
            {
                OpenPause();
            }
            else
            {
                GoBack();
            }
        };
    }

    public void OpenPause()
    {
        if (!PauseMenu.active & CurrentMenu != PauseMenu)
        {
            PauseMenu.SetActive(true);
            CurrentMenu = PauseMenu;
        }
        else if (true)
        {

        }
    }

    public void ChangeMenu(GameObject CurrentMenu, GameObject NextMenu, bool IsPause)
    {
        CurrentMenu.SetActive(false);
        NextMenu.SetActive(true);
        Time.timeScale = IsPause ? 0 : 1;

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
        PreviousMenu.SetActive(true);
    }
}