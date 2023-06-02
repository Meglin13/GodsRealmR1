using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace UI
{
    public class InGameUIScript : UIScript
    {
        private Button InventoryBT;
        private Button CharBT;
        private Button PauseBT;
        internal override void OnBind()
        {
            base.OnBind();

            InventoryBT = root.Q<Button>("InventoryBT");
            CharBT = root.Q<Button>("CharBT");
            PauseBT = root.Q<Button>("PauseBT");

            InventoryBT.clicked += () =>UIManager.Instance.OpenMenu(UIManager.Instance.Inventory);
            CharBT.clicked += () =>UIManager.Instance.OpenMenu(UIManager.Instance.CharMenu);
            PauseBT.clicked += () =>UIManager.Instance.OpenMenu(UIManager.Instance.PauseMenu);
        }
    }
}
