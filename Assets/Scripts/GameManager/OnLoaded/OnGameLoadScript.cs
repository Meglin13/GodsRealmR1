using UnityEngine;
using UnityEngine.Localization.Settings;

public class OnGameLoadScript : MonoBehaviour
{
    void Awake()
    {
        InitializeSettings();
    }

    void InitializeSettings()
    {
        string resolution = PlayerPrefs.GetString("Resolution") ?? $"{Screen.width}X{Screen.height}";
        bool IsFullscreen = PlayerPrefs.GetInt("Fullscreen") == 1 ? true : false;
        int refRate = PlayerPrefs.GetInt("RefreshRate");
        int quality = PlayerPrefs.GetInt("Quality");
        int lang = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);

        SettingsScript.SetScreen(resolution, IsFullscreen, refRate);
        SettingsScript.SetQuality(quality);
        SettingsScript.SetLocalization(lang);
    }
}