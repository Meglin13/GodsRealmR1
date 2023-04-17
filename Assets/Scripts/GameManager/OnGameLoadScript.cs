using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

public class OnGameLoadScript : MonoBehaviour
{
    public List<RenderPipelineAsset> Qualities;


    //TODO: ������ �������� ����
    void Awake()
    {
        Debug.Log("Game Loaded");
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