using UnityEngine.UIElements;

namespace UI
{
    internal class BuffStationUIScript : UIScript
    {
        private Button SelectBT;
        private Modifier[] buffs;

        private Modifier BuffContext;

        internal override void OnBind()
        {
            base.OnBind();

            SelectBT = root.Q<Button>("SelectBT");
            SelectBT.clicked -= () => BuffSelect(BuffContext);
            SelectBT.clicked += () => BuffSelect(BuffContext);
        }

        private void BuffSelect(Modifier buff)
        {
            PartyManager.Instance.ApplyBuffOnTeam(buff);
        }
    }
}