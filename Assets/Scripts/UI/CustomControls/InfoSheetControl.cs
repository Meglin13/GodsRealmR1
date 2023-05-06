using Tutorial;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class InfoSheetControl : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<InfoSheetControl, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        public InfoSheet infoSheet;
        private Image Icon;
        private Label Name;

        public InfoSheetControl()
        {
            AddToClassList("infosheet");

            Icon = new Image();
            Icon.AddToClassList("infosheet-icon");
            Add(Icon);

            Name = new Label();
            Name.AddToClassList("infosheet-label");
            Name.text = "PLACEHODER";
            Add(Name);
        }

        public void SetSheet(InfoSheet sheet)
        {
            if (sheet)
            {
                infoSheet = sheet;

                Icon.sprite = sheet.Icon;
                UIManager.Instance.ChangeLabelsText(Name, sheet.Name, UIManager.Instance.TutorialTable); 
            }
        }
    }
}
