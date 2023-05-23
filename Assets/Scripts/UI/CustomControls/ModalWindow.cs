using System;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

namespace UI.CustomControls
{
    public class ModalWindow : VisualElement
    {
        #region UXML

        [Preserve]
        public new class UxmlFactory : UxmlFactory<ModalWindow, UxmlTraits>
        { }

        [Preserve]
        public new class UxmlTraits : VisualElement.UxmlTraits
        { }

        #endregion UXML

        private Label Caption;
        private Label Title;
        private VisualElement ButtonsContainer;
        public Button YesOKBT;
        public Button NoCancelBT;

        private UIManager manager;

        public ModalWindow()
        {
            manager = UIManager.Instance;

            AddToClassList("modal-window-container");

            Title = new Label();
            Title.AddToClassList("title");
            Title.text = "Title";
            Add(Title);

            Caption = new Label();
            Caption.AddToClassList("modal-window-caption");
            Caption.text = "_Description";
            Add(Caption);

            ButtonsContainer = new VisualElement();
            ButtonsContainer.AddToClassList("buttons-container");
            ButtonsContainer.name = "ModalWindowButtonsContainer";
            Add(ButtonsContainer);

            YesOKBT = new Button();
            ButtonsContainer.Add(YesOKBT);
            YesOKBT.AddToClassList("margin-auto");
            YesOKBT.text = "#Yes";
            YesOKBT.clicked += YesOKBT_clicked;

            NoCancelBT = new Button();
            ButtonsContainer.Add(NoCancelBT);
            NoCancelBT.AddToClassList("margin-auto");
            NoCancelBT.text = "#No";
            NoCancelBT.clicked += NoCancelBT_clicked;

        }

        private void NoCancelBT_clicked()
        {
            this.style.display = DisplayStyle.None;
            Accessor.SetActive(true);
        }

        private void YesOKBT_clicked()
        {
                Success?.Invoke(); 

            Accessor.SetActive(true);
            this.style.display = DisplayStyle.None;
        }

        private Action Success;
        private GameObject Accessor;

        public void Show(ModalWindowType type, string Caption, string Title, Action Success, GameObject Accessor)
        {
            this.Success = Success;
            this.Accessor = Accessor;

            Accessor.SetActive(false);

            this.style.display = DisplayStyle.Flex;

            manager.ChangeLabelsText(this.Title, Title, UIManager.Instance.UITable);
            manager.ChangeLabelsText(this.Caption, Caption, UIManager.Instance.UITable);

            switch (type)
            {
                case ModalWindowType.YesNo:
                    manager.ChangeLabelsText(YesOKBT, "Yes", manager.UITable);
                    manager.ChangeLabelsText(NoCancelBT, "No", manager.UITable);
                    break;

                case ModalWindowType.OKCancel:
                    manager.ChangeLabelsText(YesOKBT, "OK", manager.UITable);
                    manager.ChangeLabelsText(NoCancelBT, "Cancel", manager.UITable);
                    break;

                case ModalWindowType.OK:
                    NoCancelBT.style.display = DisplayStyle.None;
                    manager.ChangeLabelsText(YesOKBT, "OK", manager.UITable);
                    break;
            }
        }
    }
}
