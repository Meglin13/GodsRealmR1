using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        //Button
        private Button SaveBT;

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
            SaveBT = root.Q<Button>("SaveBT");
            var ResetSaveBT = root.Q<Button>("ResetSaveBT");
            ResetSaveBT.clicked += () =>
            {
                //manager.ShowModalWindow(ModalWindowType.YesNo,
                //    "#Delete_Save_Caption", "#Delete_Save_Title",
                //    () => 
                SaveLoadSystem.SaveLoadSystem.DeleteSave();
                    //);
            };

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
            LangDDF.value = LocalizationSettings.SelectedLocale.LocaleName;

            //Events
            SaveBT.clicked += SaveButtonClicked;

            ResolutionDDF.RegisterValueChangedCallback(v => Resolution = v.newValue);
            RefreshRateDDF.RegisterValueChangedCallback(v => int.TryParse(v.newValue, out RefreshRate));
            QualityDDF.RegisterValueChangedCallback(v => Quality = QualityDDF.choices.IndexOf(v.newValue));
            LangDDF.RegisterValueChangedCallback(v => Lang = LangDDF.choices.IndexOf(v.newValue));

            FullscreenTG.RegisterValueChangedCallback(v => IsFullscreen = v.newValue);
        }

        private void OnDisable()
        {
            SaveBT.clicked -= SaveButtonClicked;
        }

        void SaveButtonClicked()
        {
            SettingsScript.SetScreen(Resolution, IsFullscreen, RefreshRate);
            SettingsScript.SetQuality(Quality);
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
            }

            return RefRateList.OrderBy(x => x).Distinct().ToList();
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