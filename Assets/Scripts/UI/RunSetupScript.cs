using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class RunSetupScript : UIScript
    {
        public GameObject CustomRunSetup;
        public GameObject PartySetup;

        internal override void OnBind()
        {
            base.OnBind();

            root.Q<Button>("TutorialBT").clicked += () => UIManager.Instance.ChangeScene("TutorialScene", gameObject);
            root.Q<Button>("EasyBT").clicked += () => ChooseDifficulty(1);
            root.Q<Button>("NormalBT").clicked += () => ChooseDifficulty(3);
            root.Q<Button>("HardBT").clicked += () => ChooseDifficulty(5);
            root.Q<Button>("ImpossibleBT").clicked += () => ChooseDifficulty(13);

#if !UNITY_EDITOR
            root.Q<Button>("CustomBT").style.display = DisplayStyle.None;
            root.Q<Button>("CustomBT").clicked += () => ChooseDifficulty(0);
#endif
        }

        private void ChooseDifficulty(int dif)
        {
            RunManager.SetDifficulty(dif);
            manager.ChangeMenu(gameObject, dif == 0 ? CustomRunSetup : PartySetup);
        }
    }
}