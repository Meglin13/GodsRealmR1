using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class DeathScreenScript : UIScript
    {
        internal override void OnBind()
        {
            base.OnBind();

            var RestartBT = root.Q<Button>("RestartBT");
            RestartBT.clicked += RestartBT_clicked;

            var ReturnToHub = root.Q<Button>("ReturnToHub");
            ReturnToHub.clicked += ReturnToHub_clicked;

            var ScoreLB = root.Q<Button>("ScoreLB");
            UIManager.Instance.ChangeLabelsText(ScoreLB, "Score", UITable);
            ScoreLB.text += $" {InventoryScript.Instance.Score}";
        }

        private void ReturnToHub_clicked() => UIManager.Instance.ChangeScene("HubScene", gameObject);

        private void RestartBT_clicked() => UIManager.Instance.ChangeScene(SceneManager.GetActiveScene().name, gameObject);
    }
}