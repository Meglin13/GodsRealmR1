using System.Collections.Generic;
using System.Linq;
using Tutorial;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    internal class TutorialScreenScript : UIScript
    {
        private List<InfoSheet> infoSheets;
        private VisualElement SheetsContainer;
        private Label InfoName;
        private Label InfoDesc;
        private VisualElement Image;

        internal override void OnBind()
        {
            base.OnBind();

            infoSheets = Resources.LoadAll<InfoSheet>("ScriptableObjects/TextInfo/Tutorial").ToList();

            SheetsContainer = root.Q<VisualElement>("SheetsContainer");
            InfoName = root.Q<Label>("NameLB");
            InfoDesc = root.Q<Label>("DescLB");
            Image = root.Q<VisualElement>("SheetImage");

            LoadInfo();
            SelectSheet(infoSheets[0]);
        }

        private void LoadInfo()
        {
            SheetsContainer.Clear();

            foreach (var sheet in infoSheets)
            {
                InfoSheetControl infoSheet = new InfoSheetControl();
                SheetsContainer.Add(infoSheet);

                infoSheet.SetSheet(sheet);
                infoSheet.AddManipulator(new Clickable(() => SelectSheet(infoSheet.infoSheet)));
            }
        }

        private void SelectSheet(InfoSheet infoSheet)
        {
            Image.style.backgroundImage = new StyleBackground(infoSheet.Image);

            UIManager.Instance.ChangeLabelsText(InfoName, infoSheet.Name, TutorialTable);
            UIManager.Instance.ChangeLabelsText(InfoDesc, infoSheet.Description, TutorialTable);
        }
    }
}