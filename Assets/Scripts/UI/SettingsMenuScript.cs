using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SettingsMenuScript : MonoBehaviour
{
    //Screen
    private string Resolution;
    private bool IsFullscreen;
    private int RefreshRate;
    private int Quality;
    public RenderPipelineAsset[] QualityLevels = new RenderPipelineAsset[4];

    //Button
    private Button BackButton;
    private Button SaveButton;

    //Toggle
    private Toggle FullscreenTG;

    //Dropdown
    private DropdownField ResolutionDDF;
    private DropdownField RefreshRateDDF;
    private DropdownField QualityDDF;

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        //Buttons
        BackButton = root.Q<Button>("BackBT");
        SaveButton = root.Q<Button>("SaveBT");

        //Toggles
        FullscreenTG = root.Q<Toggle>("FullscreenTG");
        FullscreenTG.value = Screen.fullScreen;

        //Dropdown Fields
        ResolutionDDF = root.Q<DropdownField>("ResDDF");
        RefreshRateDDF = root.Q<DropdownField>("RefRateDDF");
        QualityDDF = root.Q<DropdownField>("QualityDDF");

        ResolutionDDF.choices = new List<string>() { "1280X720", "1366X768", "1980X1080", "2560X1440" };
        RefreshRateDDF.choices = new List<string>() { "30", "60", "75", "144" };
        QualityDDF.choices = new List<string>() { "Low", "Medium", "High", "Ultra" };

        ResolutionDDF.value = PlayerPrefs.GetString("Resolution");
        RefreshRateDDF.value = PlayerPrefs.GetInt("RefreshRate").ToString();
        QualityDDF.value = QualityDDF.choices[PlayerPrefs.GetInt("Quality")];

        //Events
        BackButton.clicked += BackButtonClicked;
        SaveButton.clicked += SaveButtonClicked;

        ResolutionDDF.RegisterValueChangedCallback(v => Resolution = v.newValue);
        RefreshRateDDF.RegisterValueChangedCallback(v => RefreshRate = Int32.Parse(v.newValue));
        QualityDDF.RegisterValueChangedCallback(v => Quality = QualityDDF.choices.IndexOf(v.newValue));

        FullscreenTG.RegisterValueChangedCallback(v => IsFullscreen = v.newValue);
    }

    void SaveButtonClicked()
    {
        SettingsScript.SetScreen(Resolution, IsFullscreen, RefreshRate);
        SettingsScript.SetQuality(Quality, QualityLevels[Quality]);

        //Save Values
        PlayerPrefs.SetString("Resolution", Resolution);
        PlayerPrefs.SetInt("RefreshRate", RefreshRate);
        PlayerPrefs.SetInt("Fullscreen", IsFullscreen ? 1 : 0);
        PlayerPrefs.SetInt("Quality", Quality);
    }

    void BackButtonClicked()
    {
        UIManager.Instance.GoBack();
    }
}