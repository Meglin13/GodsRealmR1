using UnityEngine.UIElements;

namespace UI
{
    public class CustomRunSettingsScript : UIScript
    {
        private int Difficulty = 0;
        private RunParameters Params = new RunParameters();

        private Button NextBT;
        private Button ResetBT;

        private TextField SeedTB;
        private TextField DiffTB;

        internal override void OnBind()
        {
            base.OnBind();

            NextBT = root.Q<Button>("NextBT");
            NextBT.clicked += SaveParams;

            ResetBT = root.Q<Button>("ResetBT");
            ResetBT.clicked += ResetParams;

            SeedTB = root.Q<TextField>("SeedTB");
            int seed = 0;
            SeedTB.RegisterValueChangedCallback(v => int.TryParse(v.newValue, out seed));
            Params.Seed = seed;

            DiffTB = root.Q<TextField>("DifficultyTB");
            DiffTB.value = RunManager.Difficulty.ToString();
            int diff = 0;
            SeedTB.RegisterValueChangedCallback(v => int.TryParse(v.newValue, out diff));
            Difficulty = diff;
        }

        public void OnDisable()
        {
            NextBT.clicked -= SaveParams;
            ResetBT.clicked -= ResetParams;
        }

        private void ResetParams()
        {
            SeedTB.value = string.Empty;
        }

        public void SaveParams()
        {
            if (Difficulty > 0)
            {
                RunManager.SetDifficulty(Difficulty, Params);
            }
        }
    }
}