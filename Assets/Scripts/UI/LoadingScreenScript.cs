using EasyTransition;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class LoadingScreenScript : UIScript
    {
        private ProgressBar progressBar;

        internal override void OnBind()
        {
            base.OnBind();

            progressBar = root.Q<ProgressBar>("ProgressBar");
        }

        public void LoadScene(string SceneName)
        {
            StartCoroutine(LoadSceneAsync(SceneName));
        }

        private IEnumerator LoadSceneAsync(string SceneName)
        {
            AsyncOperation loadScene = SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);


            loadScene.allowSceneActivation = false;

            while (!loadScene.isDone)
            {
                progressBar.value = Mathf.Clamp01(loadScene.progress / 0.9f);

                if (loadScene.progress >= 0.9f)
                    loadScene.allowSceneActivation = true;

                yield return null;
            }
        }
    }
}
