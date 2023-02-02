using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public static class SettingsScript
{
    public static void SetScreen(string Resolution, bool IsFullscreen, int RefreshRate)
    {
        if (Resolution == null)
        {
            Resolution = $"{Screen.width}X{Screen.height}";
        }

        string[] res = Resolution.Split('X', StringSplitOptions.RemoveEmptyEntries);

        Application.targetFrameRate = RefreshRate;

        Screen.SetResolution(Int32.Parse(res[0]), Int32.Parse(res[1]), IsFullscreen);

        Debug.Log($"Current resolution {Screen.width}X{Screen.height}. Fullscreen {Screen.fullScreen}" +
            $"Current refreshrate {Application.targetFrameRate}. " +
            $"\nCurrent Quality {QualitySettings.GetQualityLevel()}");
    }

    public static void SetQuality(int Value, RenderPipelineAsset asset)
    {
        QualitySettings.SetQualityLevel(Value);
        QualitySettings.renderPipeline = asset;
    }
}
