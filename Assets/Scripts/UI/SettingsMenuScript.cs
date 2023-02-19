using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

namespace UI
{
    [RequireComponent(typeof(UIDocumentLocalization))]
    public class SettingsMenuScript : UIScript
    {
        //Screen
        private string Resolution;
        private bool IsFullscreen;
        private int RefreshRate;
        private int Quality;
        private int Lang;
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
        private DropdownField LangDDF;

        internal override void OnBind()
        {
            base.OnBind();

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
            LangDDF = root.Q<DropdownField>("LangDDF");

            RefreshRateDDF.choices = GetRefreshRate();
            QualityDDF.choices = new List<string>() { "Low", "Medium", "High", "Ultra" };
            LangDDF.choices = GetLocales();
            ResolutionDDF.choices = GetMonitorResolutions();

            ResolutionDDF.value = PlayerPrefs.GetString("Resolution") ?? $"{Screen.width}X{Screen.height}";
            RefreshRateDDF.value = Screen.currentResolution.refreshRate.ToString();
            QualityDDF.value = QualityDDF.choices[PlayerPrefs.GetInt("Quality")];
            LangDDF.value = LocalizationSettings.AvailableLocales.Locales[Lang].LocaleName;

            //Events
            BackButton.clicked += GoBack;
            SaveButton.clicked += SaveButtonClicked;

            ResolutionDDF.RegisterValueChangedCallback(v => Resolution = v.newValue);
            RefreshRateDDF.RegisterValueChangedCallback(v =>
            {
                int value = 60;
                int.TryParse(v.newValue, out value);
                RefreshRate = value;
            });

            QualityDDF.RegisterValueChangedCallback(v => Quality = QualityDDF.choices.IndexOf(v.newValue));
            LangDDF.RegisterValueChangedCallback(v => Lang = LangDDF.choices.IndexOf(v.newValue));

            FullscreenTG.RegisterValueChangedCallback(v => IsFullscreen = v.newValue);
        }

        void SaveButtonClicked()
        {
            SettingsScript.SetScreen(Resolution, IsFullscreen, RefreshRate);
            SettingsScript.SetQuality(Quality, QualityLevels[Quality]);
            SettingsScript.SetLocalization(Lang);
        }

        List<string> GetMonitorResolutions()
        {
            List<string> ResolutionsList = new List<string>();

            foreach (var res in Screen.resolutions)
            {
                ResolutionsList.Add($"{res.width}X{res.height}");
            }

            return ResolutionsList;
        }

        List<string> GetRefreshRate()
        {
            List<string> RefRateList = new List<string>();

            foreach (var res in Screen.resolutions)
            {
                RefRateList.Add(res.refreshRate.ToString());
                Debug.Log(res.refreshRate);
            }

            return RefRateList.Distinct().ToList();
        }

        List<string> GetLocales()
        {
            List<string> LocalesList = new List<string>();

            foreach (var loc in LocalizationSettings.AvailableLocales.Locales)
            {
                LocalesList.Add(loc.LocaleName);
            }

            return LocalesList;
        }
    }
}