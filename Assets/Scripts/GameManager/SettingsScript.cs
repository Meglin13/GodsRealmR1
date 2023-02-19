using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;

public static class SettingsScript
{
    public static void SetScreen(string resolution, bool IsFullscreen, int refreshRate)
    {
        string[] res = resolution.Split('X', StringSplitOptions.RemoveEmptyEntries);

        Application.targetFrameRate = refreshRate;

        Screen.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), IsFullscreen);

        PlayerPrefs.SetString("Resolution", resolution);
        PlayerPrefs.SetInt("RefreshRate", refreshRate);
        PlayerPrefs.SetInt("Fullscreen", IsFullscreen ? 1 : 0);
    }

    public static void SetQuality(int index, RenderPipelineAsset asset)
    {
        QualitySettings.SetQualityLevel(index);
        QualitySettings.renderPipeline = asset;

        PlayerPrefs.SetInt("Quality", index);
    }

    public static void SetLocalization(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetInt("Lang", index);
    }
}