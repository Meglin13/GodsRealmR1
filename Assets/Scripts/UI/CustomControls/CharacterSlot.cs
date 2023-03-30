using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class CharacterSlot : VisualElement
{
    #region UXML

    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlotControl, UxmlTraits>
    { }

    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits
    { }

    #endregion UXML

    public Image CharIcon;
    public Label CharName;

    public CharacterSlot()
    {
        CharIcon = new Image();
        CharName = new Label();
    }
}